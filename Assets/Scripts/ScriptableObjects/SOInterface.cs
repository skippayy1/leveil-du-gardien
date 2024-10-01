using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// #tp4 luka 
/// cette classe permet de gerer l'interface l'interface pendant le niveau de jeu
/// </summary>
public class GestionInterface : MonoBehaviour
{

    [SerializeField] TMP_Text _champPointage; // #tp4 luka champ de texte pour le pointage
    [SerializeField] TMP_Text _champEngrenages; // #tp4 luka champ de texte pour les engrenages
    [SerializeField] SOPerso _donneesPerso; // #tp4 luka scriptable object du personnage


    /// <summary>
    /// #tp4 luka 
    /// Methode pour mettre a jour l'interface (appelee par event)
    /// </summary>
    void MettreAJourInterface() 
    {
        _champPointage.text = "Pointage: " + _donneesPerso.pointage;
        _champEngrenages.text = "Engrenages: " + _donneesPerso.engrenages;
    }

}
