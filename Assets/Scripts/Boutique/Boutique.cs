using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// #tp3 luka
/// Cette classe sert a gerer la boutique et ses objets
/// ainsi que la communication avec le personnage et son inventaire
/// </summary>
public class Boutique : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; // #tp3 luka acces au scriptable object de perso
    public SOPerso donneesPerso => _donneesPerso; // #tp3 luka getter pour les donnees du personnage
    [SerializeField] TextMeshProUGUI _champArgent; // #tp3 luka acces au champ de texte de l'argent
    [SerializeField] TextMeshProUGUI _champPointage; // #tp3 luka acces au champ de texte du pointage

    /// <summary>
    /// #tp3 luka
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        MettreAJourInfo();
        _donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfo); // #tp3 luka ajout de de l'ecouteur pour un appel de la methode
    }

    /// <summary>
    /// #tp3 luka 
    /// cette methode permet de mettre a jour les infos du perso dans
    /// l'interface de la boutique
    /// </summary>
    private void MettreAJourInfo()
    {
        _champArgent.text = _donneesPerso.engrenages + " engrenages";
        _champPointage.text = _donneesPerso.pointage + " points";
    }

    /// <summary>
    /// Callback sent to all game objects before the application is quit.
    /// </summary>
    void OnApplicationQuit()
    {
        _donneesPerso.InitialiserJeu();
        _donneesPerso.InitialiserNiveau();
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        _donneesPerso.evenementMiseAJour.RemoveAllListeners(); // #tp3 luka enleve les listeners a la destruction du gameobject
    }
}
