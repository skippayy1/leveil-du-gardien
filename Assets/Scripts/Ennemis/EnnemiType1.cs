using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe de l'ennemi de type 1
/// Cette classe hérite de la classe EnnemiBase
/// Cet ennemi saute sur le joueur pour l'attaquer
/// #synthese Victor 
/// </summary>
public class EnnemiType1 : EnnemiBase
{
    [SerializeField] float _forceSaut = 1f; // Force du saut de l'ennemi
    [SerializeField] float _distanceAttaque = 2; // Distance d'attaque de l'ennemi
    [SerializeField] float _delaiAttaque = 0.5f; // Délai avant l'attaque de l'ennemi
    [SerializeField] float _delaiPause = 1f; // Délai après l'attaque de l'ennemi avant de pouvoir attaquer à nouveau
    [SerializeField] Vector2 _decalageAttaque = new(0, 0.5f); // Décalage de l'attaque de l'ennemi

    bool _attaqueEnCours; // Booléen pour savoir si l'ennemi est en train d'attaquer
    const float _VALEUR_DOT_HAUT = 0.9f; //Determine à quel point le sprite doit être à plat pour être considérer comme sur le dessus d'une platforme

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // Si l'ennemi est en haut de sa platforme et prêt du joueur, il attaque
        bool estEnHaut = Vector3.Dot(_transformSprite.up, Vector3.up) > _VALEUR_DOT_HAUT;
        bool estPretDuJoueur = Vector3.Distance(_transformSprite.position, _perso.position) < _distanceAttaque;
        if (estEnHaut && estPretDuJoueur && !_attaqueEnCours) StartCoroutine(Attaque());
    }

    /// <summary>
    /// Coroutine d'attaque de l'ennemi
    /// </summary>
    IEnumerator Attaque()
    {
        _animator.SetBool("Pret", true);
        _vitesse = 0;
        _attaqueEnCours = true;

        yield return new WaitForSeconds(_delaiAttaque);
        _animator.SetTrigger("Attaque");
        Sauter();

        yield return new WaitForSeconds(_delaiPause);
        _attaqueEnCours = false;
    }

    /// <summary>
    /// Méthode pour faire sauter l'ennemi en direction du joueur
    /// </summary>
    void Sauter()
    {
        Vector3 force = ((_perso.position - _transformSprite.position).normalized + (Vector3)_decalageAttaque) * _forceSaut;
        StartCoroutine(Projeter(force));
    }
}
