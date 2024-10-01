using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Classe de gestion du panneau des points bonus
/// Multiplie et additionne les bonus
/// #tp4 Victor
/// </summary>
public class PanneauPointsBonus : MonoBehaviour
{
    [SerializeField] float _vitesseAffichage = 1f; // #tp4 Victor Vitesse d'affichage des bonus
    [SerializeField] TextMeshProUGUI _champBonusTempsMulti; // #tp4 Victor Champ pour afficher le multiplicateur de bonus de temps
    [SerializeField] TextMeshProUGUI _champBonusTemps; // #tp4 Victor Champ pour afficher le bonus de temps
    [SerializeField] TextMeshProUGUI _champBonusRecoltesMulti; // #tp4 Victor Champ pour afficher le multiplicateur de bonus de recoltes
    [SerializeField] TextMeshProUGUI _champBonusRecoltes; // #tp4 Victor Champ pour afficher le bonus de recoltes
    [SerializeField] TextMeshProUGUI _champTotal; // #tp4 Victor Champ pour afficher le total des bonus
    [SerializeField] Button _boutonContinuer; // #tp4 Victor Bouton pour continuer

    int _tempsRestant;
    public int tempsRestant
    {
        get { return _tempsRestant;}
        set { _tempsRestant = value;}
    }

    int _nbBonusRecolte;
    public int nbBonusRecolte
    {
        get { return _nbBonusRecolte;}
        set { _nbBonusRecolte = value;}
    }
    
    void Start()
    {
        StartCoroutine(CoroutAfficherBonus());
    }

    /// <summary>  
    /// Coroutine pour afficher les bonus
    /// #tp4 Victor
    /// </summary>
    IEnumerator CoroutAfficherBonus()
    {
        int bonusTemps = _tempsRestant * 100;
        int bonusRecoltes = _nbBonusRecolte * 10;
        yield return new WaitForSeconds(_vitesseAffichage);
        _champBonusTempsMulti.text = $"{_tempsRestant} x 100";
        yield return new WaitForSeconds(_vitesseAffichage);
        _champBonusTemps.text = $"+{bonusTemps}";
        yield return new WaitForSeconds(_vitesseAffichage);
        _champBonusRecoltesMulti.text = $"{_tempsRestant} x 10";
        yield return new WaitForSeconds(_vitesseAffichage);
        _champBonusRecoltes.text = $"+{_nbBonusRecolte}";
        yield return new WaitForSeconds(_vitesseAffichage);
        _champTotal.text = $"{bonusTemps + bonusRecoltes}";
        yield return new WaitForSeconds(_vitesseAffichage * 2);
        _boutonContinuer.interactable = true;
    }
}
