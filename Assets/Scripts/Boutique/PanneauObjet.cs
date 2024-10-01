using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// #tp3 luka
/// cette classe sert a gerer le panneau d'objet de la boutique, d'y ajouter chaque objet
/// et d'acceder aux autres classes pour gerer l'argent et autres valeurs
/// </summary>
public class PanneauObjet : MonoBehaviour
{
    [Header("Les données")]
    [SerializeField] SOObjet _donnees; // #tp3 luka acces au scriptable object de l'objet
    public SOObjet donnees => _donnees; // #tp3 luka getter pour les donnees de l'objet
    [SerializeField] Boutique _boutique; // #tp3 luka acces au script de la boutique
    public Boutique boutique => _boutique; // #tp3 luka getter pouur la boutique
    [Header("Les conteneurs")]
    [SerializeField] TextMeshProUGUI _champNom; // #tp3 luka champ de texte pour le nom
    [SerializeField] TextMeshProUGUI _champPrix; // #tp3 luka champ de texte pour le prix
    [SerializeField] TextMeshProUGUI _champDescription; // #tp3 luka champ de texte pour la description
    [SerializeField] Image _image; // #tp3 luka champ serialise pour l'image
    [SerializeField] CanvasGroup _canvasGroup; // #tp3 luka champ serialise pour le canvasgroup
    [SerializeField] bool _estAchete; // #tp3 luka verifie si l'objet est achete
    void Start()
    {
        MettreAJourInfo();
        boutique.donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfo); // #tp3 luka ajout d'un listener pour l'evenement de mise a jour
    }

    /// <summary>
    /// #tp3 luka
    /// cette methode permet de mettre a jour les informations 
    /// de la vignette de lobjet quand appelee
    /// </summary>
    void MettreAJourInfo()
    {
        // #tp3 luka assigner les valeurs du scriptable object a son objet
        _champNom.text = _donnees.nom;
        if (_champPrix != null) _champPrix.text = _donnees.prix + " engrenages";
        if (_champDescription != null) _champDescription.text = _donnees.description;
        _image.sprite = _donnees.sprite;
        _estAchete = _donnees.estAchete;
        GererDispo();
    }

    /// <summary>
    /// #tp3 luka
    /// cette methode permet de gerer la disponibilite de l'objet en se basant
    /// sur la quantite d'engrenages que possede le joueur
    /// </summary>
    void GererDispo()
    {

        bool aAssezArgent = boutique.donneesPerso.engrenages >= _donnees.prix; // #tp3 luka verifie si le joueur possede plus d'engrenages que le prix de l'objet
        // #tp3 luka si le joueur n'a pas assez d'argent desactiver le bouton et reduire le alpha de la vignette
        if (aAssezArgent)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1;
        }
        else
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0.5f;

        }
    }
/// <summary>
/// #tp3 luka 
/// cette methode permet de reduire la quantite d'engrenages dans l'inventaire du joueur
/// quand il achete un objet
/// </summary>
    public void Acheter()
    {
        _donnees.estAchete = true;
        boutique.donneesPerso.Acheter(_donnees);
    }

}
