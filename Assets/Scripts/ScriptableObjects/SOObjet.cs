using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objet", menuName = "Objet boutique", order = 53)]
/// <summary>
/// #tp3 luka
/// ce scriptable object permet de storer et gerer
/// les valeurs des objets de la boutique
/// </summary>
public class SOObjet : ScriptableObject
{
    [Header("LES DONNÉES")]
    [SerializeField] string _nom = "Ceinture"; // #tp3 luka nom de l'objet
    [SerializeField, Tooltip("Icone de l'objet a afficher dans la boutique")] Sprite _sprite; // #tp3 luka icone de l'objet dans la boutique
    [SerializeField][Range(0, 200)] int _prix = 30; // #tp3 luka prix de l'objet
    [SerializeField, TextArea] string _description; // #tp3 luka description de l'objet
    [SerializeField] bool _estAchete = false; // #tp3 luka verifie si l'objet

    public string nom { get => _nom; set => _nom = value; } // #tp3 luka getter et setter pour la valeur du nom de l'objet
    public Sprite sprite { get => _sprite; set => _sprite = value; } // #tp3 luka getter et setter pour la valeur de l'icone de l'objet
    public int prix { get => _prix; set => _prix = Mathf.Clamp(value, 0, int.MaxValue); } // #tp3 luka getter et setter pour la valeur du prix de l'objet
    public string description { get => _description; set => _description = value; } // #tp3 luka getter et setter pour la valeur de la description de l'objet
    public bool estAchete { get => _estAchete; set => _estAchete = value; } // #tp3 luka getter et setter pour verifier si l'objet est achete ou non
}