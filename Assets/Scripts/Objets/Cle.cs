using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// #tp3 luka 
/// cette classe permet de gerer l'interactivite de la cle avec le personnage 
/// </summary>
public class Cle : MonoBehaviour
{
    [SerializeField] SOPerso _perso; // #tp3 luka acces au scriptable object pour les donnees du perso
    [SerializeField] GameObject _particules; // #tp3 Victor Modèle du système de particules de rétroaction
    [SerializeField] Retroaction _retroModele; // #tp3 Victor Modèle du champ de rétroaction
    [SerializeField] AudioClip _son; // #tp4 Victor Son de la clé

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // #tp3 luka verifie s'il y a collision avec le joueur
        {
           _perso.possedeCle = true;
            Instantiate(_particules, transform.position, Quaternion.identity); // #tp3 Victor Instanciation du système de particules de rétroaction
            // #tp3 Victor Instanciation du champ de rétroaction
            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent);
            retro.ChangerTexte("Clé récupérée!");
            GestMusique.instance.ActiverPisteEvenA();
            GestSons.instance.JouerSon(_son, 1f);
            Destroy(gameObject);
        }
    }
}
