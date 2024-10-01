using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// #tp3 luka
/// cette classe permet de gerer l'interactivite entre la porte et le personnage
/// </summary>
public class Porte : MonoBehaviour
{
    [SerializeField] SONavigation _laNavigation; // #tp3 luka acces au scriptable object de la navigation afin de changer la scene si les conditions sont remplis
    [SerializeField] SOPerso _perso; // #tp3 luka acces au scriptable object des donnees du perso

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            VerifierPossessionCle(_perso);
            Debug.Log("col avec porte");
        }

    }
/// <summary>
/// #tp3 luka
/// cette methode permet de verifier si le personnage est bel et bien en possession de la cle
/// </summary>
/// <param name="possedeCle"></param>
    void VerifierPossessionCle(bool possedeCle)
    {
        possedeCle = _perso.possedeCle; // #tp3 luka verifie si le personnage possede la cle dans le niveau
        if (possedeCle)
        {
            _laNavigation.RentrerBoutique();
        }
        else
        {
            Debug.Log("Le joueur ne possede pas la cle");
        }
    }
}
