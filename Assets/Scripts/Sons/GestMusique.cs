using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe de gestion de la musique
/// #tp4 Victor
/// </summary>
public class GestMusique : MonoBehaviour
{
    [SerializeField] AudioSource _sourcePisteBase; // Source de la piste de base
    [SerializeField] AudioSource _sourcePisteEvenA; // #tp4 Victor Source de la piste pour l'événement A
    [SerializeField] AudioSource _sourcePisteEvenB; // #tp4 Victor Source de la piste pour l'événement B
    [SerializeField] float _dureeFondu = 5f; // #tp4 Victor Durée du fondu

    static GestMusique _instance; // #tp4 Victor Singleton
    public static GestMusique instance => _instance;

    void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
    }

    public void ActiverPisteEvenA()
    {
        StartCoroutine(EnumFaireFonduMusique(_sourcePisteBase, 0f));
        StartCoroutine(EnumFaireFonduMusique(_sourcePisteEvenA, 1f));
        StartCoroutine(EnumFaireFonduMusique(_sourcePisteEvenB, 0f));
    }

    public void ActiverPisteEvenB()
    {
        StartCoroutine(EnumFaireFonduMusique(_sourcePisteBase, 0f));
        StartCoroutine(EnumFaireFonduMusique(_sourcePisteEvenA, 0f));
        StartCoroutine(EnumFaireFonduMusique(_sourcePisteEvenB, 1f));
    }
        

    /// <summary>
    /// Coroutine pour faire un fondu enchaîné de la musique
    /// #tp4 Victor
    /// </summary>
    IEnumerator EnumFaireFonduMusique(AudioSource source, float volumeCible)
    {
        float volumeInitial = source.volume;
        float tempsEcoule = 0f;

        while (tempsEcoule < _dureeFondu)
        {
            source.volume = Mathf.Lerp(volumeInitial, volumeCible, tempsEcoule / _dureeFondu);
            tempsEcoule += Time.deltaTime;
            yield return null;
        }

        source.volume = volumeCible;
    }
}
