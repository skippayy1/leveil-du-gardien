using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Classe de gestion des panneaux de joueur présents dans le tableau d'honneur
/// Affiche le classement, le score et enregistre les nouveau score avec le nom
/// #tp4 Victor
/// </summary>
public class PanneauJoueur : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textClassement; // #tp4 Victor Champ pour afficher le classement
    [SerializeField] TMP_InputField _champNom; // #tp4 Victor Champ pour entrer le nom du joueur
    [SerializeField] GameObject _boutonEnregistrer; // #tp4 Victor Bouton pour enregistrer le score
    [SerializeField] TextMeshProUGUI _textScore; // #tp4 Victor Champ pour afficher le score
    [SerializeField] SOPerso _donneesPerso; // #tp4 Victor Scriptable object pour les données du personnage
    [SerializeField] SOSauvegarde _sauvegarde; // #tp4 Victor Scriptable object pour la sauvegarde des scores

    int _classement;
    public int classement
    {
        get => _classement;
        set{ _classement = value; }
    }

    string _nom;
    public string nom
    {
        get => _nom;
        set{ _nom = value; }
    }

    bool _scoreEstNouveau;
    public bool scoreEstNouveau
    {
        get => _scoreEstNouveau;
        set{ _scoreEstNouveau = value; }
    }

    int _score;
    public int score
    {
        get => _score;
        set{ _score = value; }
    }

    /// <summary>  
    /// Affiche le score du joueur dans le tableau d'honneur
    /// #tp4 Victor
    /// </summary>
    public void AfficherScore()
    {
        _textClassement.text = _classement.ToString();
        _champNom.text = _nom;
        _champNom.interactable = _scoreEstNouveau;
        _champNom.gameObject.GetComponent<Image>().enabled = _scoreEstNouveau;
        _boutonEnregistrer.SetActive(_scoreEstNouveau);
        _textScore.text = _score.ToString();
    }

    /// <summary>
    /// Enregistre le score du joueur entré dans le tableau d'honneur
    /// #tp4 luka
    /// </summary>
    public void EnregistrerScore()
    {
        string nom = _champNom.text;
        _sauvegarde.AjouterResultat(nom, _donneesPerso.pointage);
        _scoreEstNouveau = false; // #tp4 luka le score n'est plus nouveau
        _champNom.interactable = false; // #tp4 luka le champ de texte n'est plus interactif
        AfficherScore();
    }
}
