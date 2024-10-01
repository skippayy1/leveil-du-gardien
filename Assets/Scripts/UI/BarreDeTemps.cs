using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #tp4 luka
/// Cette classe permet de gérer la barre de temps qui se réduit au fur et à mesure que le temps passe.
/// </summary>
public class BarreDeTemps : MonoBehaviour
{
    [SerializeField] float _tempsEcoule = 0; // #tp4 luka temps ecoule actuel
    [SerializeField] RectTransform _rt; // tp4 luka rect transform de la barre
    [SerializeField] float _duree; // #tp4 luka duree du niveau (et consequamment de la barre)
    [SerializeField] SONavigation _sONavigation; // #tp4 luka scriptable object de navigation
    [SerializeField] float _pourcentageActivationMusique = 0.8f; // #tp4 Victor Pourcentage du temps ecoule pour activer la musique
    private Vector3 grosseurBarreIni; // #tp4 luka grosseur de la barre au depart

    void Start()
    {
        grosseurBarreIni = _rt.localScale; // #tp4 luka initialisation de la grosseur de la barre
        StartCoroutine(CoroutReduireBarre());
    }

    /// <summary>
    /// #tp4 luka 
    /// coroutine qui reduit la barre de temps
    /// </summary>
    IEnumerator CoroutReduireBarre()
    {
        _tempsEcoule = 0f;
        Vector3 _vecteurRapetissement = new Vector3(0, 1, 1); // #tp4 luka vecteur pour determiner les axes de rapetissement de la barre

        while (_tempsEcoule < _duree) // #tp4 luka tant que le temps ecoule est inferieur a la duree du niveau
        {
            _rt.localScale = Vector3.Lerp(grosseurBarreIni, _vecteurRapetissement, _tempsEcoule / _duree); // #tp4 luka rappetissement de la barre 
            _tempsEcoule += Time.deltaTime; // #tp4 luka incrementation du temps ecoule
            if (_tempsEcoule > _duree * _pourcentageActivationMusique) GestMusique.instance.ActiverPisteEvenB(); // #tp4 Victor Activation de la musique lorque le temps ecoule atteint un certain pourcentage
            yield return null;
        }

        _rt.localScale = Vector3.zero; // #tp4 luka si le temps est ecoule, la barre est reduite a zero
        if (_tempsEcoule >= _duree) // #tp4 luka si le temps ecoule est superieur ou egal a la duree
        {
            _sONavigation.AllerScenePointage(); // #tp4 luka aller a la scene du menu
        }
    }


    /// <summary>
    /// #tp4 luka 
    /// methode pour ajouter du temps a la barre
    /// </summary>
    public void AjouterTemps(float temps)
    {
        _duree += temps;
    }
}
