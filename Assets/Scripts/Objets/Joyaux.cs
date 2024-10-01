using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #tp3 luka
/// cette classe permet de gerer l'interactivite entre le perso et les joyaux
/// </summary>
public class Joyaux : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; // #tp3 luka acces au scriptable object du perso
    [SerializeField] int _engrenagesAjout; // #tp3 luka ajout des engrenages pour chaque joyaux de collectione
    [SerializeField] Retroaction _retroModele; // #tp3 Victor Modèle du champ de rétroaction
    [SerializeField] AudioClip _son;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            _donneesPerso.engrenages += _engrenagesAjout;
            // #tp3 Victor Instanciation du champ de rétroaction
            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent);
            retro.ChangerTexte($"+ {_engrenagesAjout} engrenages!");
            GestSons.instance.JouerSon(_son, 1f);
            Destroy(gameObject);
        }
    }
}
