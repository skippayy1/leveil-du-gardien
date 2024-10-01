using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// #synthese luka
/// cette classe sert a gerer le collider de detection de l'ennemi  
/// </summary>
public class ColliderDetectionEnnemi : MonoBehaviour
{
    [SerializeField] float _rayon; // #synthese luka rayon du collider
    CircleCollider2D _colliderDetection; // #synthese luka collider de detection
    EnnemiType2 _ennemi; // #synthese luka acces au script de l'ennemi
    void Awake()
    {
        _ennemi = GetComponentInParent<EnnemiType2>(); // #synthese luka recuperer le script de l'ennemi
        // #synthese luka ajouter un collider de detection a l'ennemi et faire ses reglages
        _colliderDetection = gameObject.AddComponent<CircleCollider2D>();
        _colliderDetection.radius = _rayon;
        _colliderDetection.isTrigger = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _ennemi.peutSuivre = true; // #synthese luka l'ennemi peut suivre le joueur
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _ennemi.peutSuivre = false; // #synthese luka l'ennemi ne peut plus suivre le joueur
        }
    }


}
