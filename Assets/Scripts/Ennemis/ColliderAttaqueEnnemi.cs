using UnityEngine;
/// <summary>
/// #synthese luka
/// cette classe sert a gerer le collider d'attaque de l'ennemi 
/// </summary>
public class ColliderAttaqueEnnemi : MonoBehaviour
{
    [SerializeField] float _rayon; // #synthese luka rayon du collider
    [SerializeField] AudioClip _sonDetection;
    CircleCollider2D _colliderAttaque; // #synthese luka collider d'attaque
    EnnemiType2 _ennemi; // #synthese luka acces au script de l'ennemi
    void Awake()
    {
        _ennemi = GetComponentInParent<EnnemiType2>(); // #synthese luka recuperer le script de l'ennemi
        // #synthese luka ajouter un collider d'attaque a l'ennemi et faire ses reglages
        _colliderAttaque = gameObject.AddComponent<CircleCollider2D>();
        _colliderAttaque.radius = _rayon;
        _colliderAttaque.isTrigger = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _ennemi.peutTirer = true; // #synthese luka l'ennemi peut tirer
            GestSons.instance.JouerSon(_sonDetection, 1f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _ennemi.peutTirer = false; // #synthese luka l'ennemi ne peut plus tirer
        }
    }
}
