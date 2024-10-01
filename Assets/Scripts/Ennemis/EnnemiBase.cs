using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe de base des ennemis
/// Cette classe est parent de toutes les classes d'ennemis
/// Elle contient les méthodes de déplacement et de rotation des ennemis
/// #synthese Victor 
/// </summary>
public class EnnemiBase : MonoBehaviour
{
    [SerializeField] protected float _vitesseInital = 1.5f; // Vitesse initiale de l'ennemi
    [SerializeField] float _vitesseRotation = 10f; // Vitesse de rotation de l'ennemi
    [SerializeField] float _gravite = 1f; // Gravité de l'ennemi
    [SerializeField] float _longueurRayon = 0.3f; // Longueur du rayon de détection de collision
    [SerializeField] LayerMask _masqueCollision; // Masque de collision
    [SerializeField] AudioClip _sonProjete; // Son lorsque l'ennemi est projeté
    [Space(10)]
    [SerializeField] SOPerso _donneesPerso; // #synthese luka scriptable object du personnage
    [SerializeField] int _forceAttaque; // #synthese luka valeur de l'attaque de l'ennemi

    protected float _vitesse; // Vitesse de l'ennemi
    protected float _direction = 1; // Direction de l'ennemi
    protected Transform _transformSprite; // Transform du sprite de l'ennemi
    protected Rigidbody2D _rb; // Rigidbody de l'ennemi
    protected SpriteRenderer _spriteRenderer; // Instance du sprite renderer
    protected Transform _perso; // Transform du personnage
    protected bool _graviteRelative = true; // Booléen pour savoir si la gravité est relative
    protected Animator _animator; // Instance de l'animator
    const float _DELAI_VERIF_SOL = 0.5f; // Delai avant la vérification de l'ennemi au sol

    public virtual void Start()
    {
        _vitesse = _vitesseInital;
        _transformSprite = GetComponentInChildren<SpriteRenderer>().transform;
        _rb = GetComponentInChildren<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _perso = _donneesPerso.perso.transform;
        _animator = GetComponentInChildren<Animator>();
    }

    public virtual void FixedUpdate()
    {
        // Gravité relative et vitesse de l'ennemi
        if (_graviteRelative) _rb.velocity = -_rb.transform.up * _gravite + _rb.transform.right * _vitesse;
        _rb.freezeRotation = !_graviteRelative;
        _animator.SetFloat("VitesseMarche", _rb.velocity.magnitude);
    }

    public virtual void Update()
    {
        // Angle de rotation du sprite avec une interpolation pour éviter les saccades
        float angle = Mathf.LerpAngle(_transformSprite.transform.eulerAngles.z, _rb.transform.eulerAngles.z, _vitesseRotation * Time.deltaTime);

        // Rotation et position du sprite
        _transformSprite.transform.rotation = Quaternion.Euler(0, 0, angle);
        _transformSprite.transform.position = _rb.transform.position;

        // Vérification de collision avec un mur pour changer de direction
        Vector2 pointDepart = _rb.transform.position - _rb.transform.right * _longueurRayon;
        if (Physics2D.Raycast(pointDepart, _rb.transform.right, _longueurRayon * 2, _masqueCollision))
        {
            _direction *= -1;
            _vitesse = _vitesseInital * _direction;
            _spriteRenderer.flipX = _direction < 0;
        }
    }

    public IEnumerator Projeter(Vector3 force)
    {
        GestSons.instance.JouerSon(_sonProjete, 1f);
        _vitesse = 0;
        _graviteRelative = false;
        _rb.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(_DELAI_VERIF_SOL);
        yield return new WaitUntil(() => Physics2D.Raycast(_transformSprite.position, -_transformSprite.up, _longueurRayon, _masqueCollision).collider != null);
        _animator.SetBool("Pret", false);
        _vitesse = _vitesseInital * _direction;
        _spriteRenderer.flipX = _direction < 0;
        _graviteRelative = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position - transform.right * _longueurRayon, _longueurRayon * 2 * transform.right);
        Gizmos.DrawRay(transform.position, Vector3.down * _longueurRayon);
    }

    /// <summary>
    /// #synthese luka
    /// cette fonction permet d'enlever de la vie au joueur
    /// </summary>
    public void EnleverVie()
    {
        _donneesPerso.vie -= _forceAttaque; // #synthese luka enlever de la vie a l'ennemi
    }
}
