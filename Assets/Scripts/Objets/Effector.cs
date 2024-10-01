using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère l'activation de l'effector et des particules associées
/// Auteur du code et des commentaires: Victor Aubry
/// #tp3 Victor
/// </summary>
public class Effector : MonoBehaviour
{
    [SerializeField] ParticleSystem _particle;
    [SerializeField] float _forceEffecteur; // Force de l'effet du AreaEffector2D
    [SerializeField] float _dureeForce; // Durée de l'effet
    [SerializeField] AreaEffector2D _effector;
    [SerializeField] AudioClip _son;
    bool _peutEtreActive = true; // Indique si l'effet peut être activé

    void OnTriggerEnter2D()
    {
        if (!_peutEtreActive) return; // Vérifie si l'effet peut être activé, sinon sort de la méthode
        _particle.Clear(); // Efface les particules précédentes
        _particle.Play(); // Joue la nouvelle particule
        StartCoroutine(ActiverEffecteur()); // Démarre une coroutine pour activer l'effet avec un délai
        GestSons.instance.JouerSon(_son, 1f);
    }

    /// <summary>
    /// Coroutine pour activer l'effet
    /// </summary>
    IEnumerator ActiverEffecteur() 
    {   
        _peutEtreActive = false; // Désactive la possibilité d'activer l'effet pendant son exécution
        _effector.forceMagnitude = _forceEffecteur; // Définit la force de l'effet sur l'AreaEffector2D
        yield return new WaitForSeconds(_dureeForce); // Attend pendant la durée spécifiée
        _effector.forceMagnitude = 0; // Réinitialise la force de l'effet à zéro
        _peutEtreActive = true; // Réactive la possibilité d'activer l'effet
    }
}
