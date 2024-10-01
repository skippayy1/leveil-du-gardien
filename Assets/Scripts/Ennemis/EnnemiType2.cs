using System.Collections;
using UnityEngine;
/// <summary>
/// #synthese luka
/// cette classe sert a gerer l'ennemi de type 2 (tirer des projectiles visees sur le joueur)
/// </summary>
public class EnnemiType2 : EnnemiBase
{
    [SerializeField] GameObject _module; // #synthese luka module de l'ennemi
    [SerializeField] float _vitesseRotationCanon = 5f; // #synthese luka vitesse de rotation du cannon
    [SerializeField] GameObject _prefabProjectile; // #synthese luka prefab du projectile
    [SerializeField] GameObject _pointDeTir; // #synthese luka point de tir du projectile
    [SerializeField] float _intervalTir = 2f; // #synthese luka intervalle de tir
    [SerializeField] Vector2 _intervallesTir = new(); // #synthese luka intervalles de tir
    [SerializeField] GameObject _objetColliderDetection; // #synthese luka objet du collider de detection
    [SerializeField] GameObject _objetColliderAttaque;  // #synthese luka objet du collider d'attaque
    Vector3 _ajustmentPosJoueur = new Vector3(0, 0.3f, 0); // #synthese luka ajustement de la position du joueur
    Collider2D _colAttaque; // #synthese luka collider d'attaque
    Collider2D _colDetection; // #synthese luka collider de detection
    bool _peutTirer; // #synthese luka l'ennemi peut tirer
    public bool peutTirer { get { return _peutTirer; } set { _peutTirer = value; } } // #synthese luka getter et setter de peutTirer
    bool _peutSuivre; // #synthese luka l'ennemi peut suivre le joueur
    public bool peutSuivre { get { return _peutSuivre; } set { _peutSuivre = value; } } // #synthese luka getter et setter de peutSuivre
    public override void Start()
    {
        base.Start();
        _intervalTir = Random.Range(_intervallesTir.x, _intervallesTir.y); // #synthese luka intervalle de tir aleatoire
        _colAttaque = _objetColliderAttaque.GetComponent<Collider2D>();
        _colDetection = _objetColliderDetection.GetComponent<Collider2D>();
        StartCoroutine(CoroutTir()); // #synthese luka lancer la coroutine de tir
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (_peutSuivre)
        {
            SuivreJoueur(); // #synthese luka suivre le joueur
        }
    }
    /// <summary>
    /// #synthese luka
    /// cette coroutine sert a tirer des projectiles a une intervalle serialize
    /// </summary>
    IEnumerator CoroutTir()
    {
        while (true)
        {
            if (_perso != null && _peutTirer == true)
            {
                Vector3 directionJoueur = _perso.transform.position - _pointDeTir.transform.position; // #synthese luka calculer la direction du joueur
                Debug.DrawRay(transform.position, directionJoueur, Color.red); // #synthese luka dessiner un rayon pour la direction

                Instantiate(_prefabProjectile, _pointDeTir.transform.position, Quaternion.LookRotation(Vector3.forward, _module.transform.up)); // #synthese luka instancier le projectile
            }

            yield return new WaitForSeconds(_intervalTir);
        }
    }
    /// <summary>
    /// #synthese luka
    /// cette methode sert a suivre le joueur
    /// </summary>
    void SuivreJoueur()
    {
        Vector3 _directionJoueur = _perso.transform.position - _pointDeTir.transform.position; // Calculate the direction to the player
        Debug.DrawRay(transform.position, _directionJoueur, Color.red);

        float angleVisee = Vector3.SignedAngle(transform.up, _directionJoueur + _ajustmentPosJoueur, Vector3.forward); // calculer l'angle de visée vers le joueur
        float angle = Mathf.MoveTowardsAngle(_module.transform.eulerAngles.z, angleVisee, _vitesseRotationCanon * Time.deltaTime); // Calculer l'angle de rotation
        _module.transform.rotation = Quaternion.Euler(0, 0, angle); // Appliquer la rotation
    }
}
