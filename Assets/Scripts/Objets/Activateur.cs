using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// #tp3 luka 
/// Cette classe permet de gerer l'activateur et ses evenements
/// </summary>
public class Activateur : MonoBehaviour
{
    [SerializeField] Sprite _spriteDesactive; // #tp3 Victor Sprite de l'activateur désactivé
    [SerializeField] GameObject _particules; // #tp3 Victor Modèle du système de particules de rétroaction
    [SerializeField] Retroaction _retroModele; // #tp3 Victor Modèle du champ de rétroaction
    [SerializeField] AudioClip _sonActivateur;

    SpriteRenderer _sr;

    void Start() => _sr = GetComponent<SpriteRenderer>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Niveau.instance.InvokerEvenementActivateur(); // #tp3 luka invocation de l'eveneemnt pour activer les bonus
            Instantiate(_particules, transform.position, Quaternion.identity); // #tp3 Victor Instanciation du système de particules de rétroaction
            // #tp3 Victor Instanciation du champ de rétroaction
            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent);
            retro.ChangerTexte("Bonus activés!");
            _sr.sprite = _spriteDesactive;
            GestSons.instance.JouerSon(_sonActivateur, 1f);
        }
    }
}
