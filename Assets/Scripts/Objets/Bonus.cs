using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// #tp3 luka 
/// cette classe permet de gerer l'interactivite entre les bonus et le joueur ainsi qu'avec l'activateur
/// </summary>
public class Bonus : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; // #tp3 luka acces au scriptable object du perso
    [SerializeField] int _pointageAjout; // #tp3 luka ajout au pointage pour chaque bonus de ramasse
    [SerializeField] TypeBonus _type; //  #tp3 luka acces du enum pour le type de bonus
    [SerializeField] GameObject _particules; // #tp3 Victor Modèle du système de particules de rétroaction
    [SerializeField] Retroaction _retroModele; // #tp3 Victor Modèle du champ de rétroaction
    [SerializeField] Sprite _spriteDesactive; // #tp3 Victor Sprite du bonus désactivé
    [SerializeField] Sprite _spriteActif; // #tp3 Victor Sprite du bonus actif
    [SerializeField] AudioClip _sonBonus;

    bool _estActif = false; // #tp3 luka bool pour verifier si le bonus est actif
    SpriteRenderer _sr; // #tp3 luka acces au spriterenderer de l'objet
    Perso _perso; // #tp3 luka acces au perso et ses methodes

    void Awake()
    {   
        _perso = FindObjectOfType<Perso>(); // #tp3 luka acceder au perso et son script
        _sr = GetComponent<SpriteRenderer>();
        VerifierSiActif();
        // #tp3 luka ajout des listeners pour l'activation
        Niveau.instance.evenementActivationBonus.AddListener(RendreActif);
        Niveau.instance.evenementActivationBonus.AddListener(VerifierSiActif);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_estActif)
            {
                // #tp3 luka sur collision avec le joueur ajouter au pointage, activer l'effet du bonus et detruire l'objet
                _donneesPerso.pointage += _pointageAjout;
                Instantiate(_particules, transform.position, Quaternion.identity); // #tp3 Victor Instanciation du système de particules de rétroaction
                if(_type == TypeBonus.Aimant)
                {
                    _perso.ActiverAimant();
                    // #tp3 Victor Instanciation du champ de rétroaction
                    Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent);
                    retro.ChangerTexte("Aimant activé!");
                }
                else if(_type == TypeBonus.Vitesse)
                {
                   _perso.ActiverBonusCourse();
                   // #tp3 Victor Instanciation du champ de rétroaction
                    Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent);
                    retro.ChangerTexte("Bonus de course activé!");
                }
                GestSons.instance.JouerSon(_sonBonus, 1f);
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// #tp3 luka
    /// cette methode permet de reagir a l'activation du bonus et de choisir un type pour le bonus
    /// </summary>
    public void RendreActif()
    {
        _estActif = true;
    }
    
    /// <summary>
    /// #tp3 luka 
    /// cette methode verifie si le bonus est actif
    /// </summary>
    public void VerifierSiActif()
    {
        _sr.sprite = _estActif? _spriteActif : _spriteDesactive;
    }
}
