using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe de la transition
/// Gère la transition entre les niveaux en fondu
/// #synthese Victor
/// </summary>
public class Transition : MonoBehaviour
{
    [SerializeField] float _vitesse = 1; // Vitesse de la transition

    SpriteRenderer _sr; // SpriteRenderer de l'objet

    // Singleton
    static Transition _instance;
    public static Transition Instance => _instance;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }

    void Start() => _sr.sharedMaterial.SetFloat("_Progression", 1);

    // Fonction de transition
    // Prend l'instance du portail en paramètre pour appeler la fonction Teleporter après la transition
    public void FaireTransition(float depart, float arrivee, Portail portail) => StartCoroutine(CoroutTransition(depart, arrivee, portail));
    public void FaireTransition(float depart, float arrivee) => StartCoroutine(CoroutTransition(depart, arrivee, null));

    // Coroutine de transition
    IEnumerator CoroutTransition(float depart, float arrivee, Portail portail)
    {
        float progression = 0;
        while (progression < 1)
        {
            progression += Time.deltaTime * _vitesse;
            _sr.sharedMaterial.SetFloat("_Progression", Mathf.Lerp(depart, arrivee, progression));
            yield return null;
        }
        portail?.TeleporterDepuis();
    }
}
