using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System.Linq;

/// <summary>
/// Classe de gestion du panneau d'honneur
/// Gère tous ses panneaux de joueur enfants
/// Cette classe est connecter avec l'object de gestion des scores
/// #tp4 Victor
/// </summary>
public class PanneauDHonneur : MonoBehaviour
{
    [SerializeField] SOSauvegarde _sauvegarde; // #tp4 luka scriptable object de sauvegarde
    [SerializeField] SOPerso _donneesPerso; // #tp4 luka scriptable object du personnage
    [SerializeField] PanneauJoueur _panneauJoueurPrefab; // #tp4 victor prefab du panneau joueur
    List<int> lesScores = new(); // #tp4 luka liste des scores
    bool nouveauScoreEstAffiche = false; // #tp4 luka bool pour verifier si le nouveau score est affiche

    void Start()
    {
        AfficherDonnees();
    }

    /// <summary>
    /// #tp4 Victor
    /// Methode pour afficher les donnees du tableau d'honneur
    /// </summary>
    // void AfficherDonnees()
    // {
    //     _sauvegarde.LireFichier();
    //     var donneesScores = _sauvegarde.ChercherListeScores();
    //     Debug.LogWarning(_sauvegarde.listeScores.Count);

    //     int plusPetitScore = donneesScores.Min(score => score.score); // #tp4 luka score le plus petit
    //     int classement = 0;

    //     // foreach (var score in donneesScores)
    //     // {
    //     //     classement++;
    //     //     if (score.score < _donneesPerso.pointage && nouveauScoreEstAffiche == false) // #tp4 luka si le score est plus petit que le score du joueur
    //     //     {
    //     //         nouveauScoreEstAffiche = true;
    //     //         PanneauJoueur panneauNouveau = Instantiate(_panneauJoueurPrefab, transform);
    //     //         panneauNouveau.scoreEstNouveau = true;
    //     //         panneauNouveau.classement = classement;
    //     //         panneauNouveau.score = _donneesPerso.pointage;
    //     //         panneauNouveau.AfficherScore();
    //     //     }
    //     //     else if (score.score == plusPetitScore && nouveauScoreEstAffiche == false) // #tp4 luka si le score du joueur est plus petit que le score preexistant le plus petit
    //     //     {
    //     //         Debug.LogWarning("creation panneau");
    //     //         PanneauJoueur panneau = Instantiate(_panneauJoueurPrefab, transform);
    //     //         panneau.nom = score.nom;
    //     //         panneau.classement = classement;
    //     //         panneau.score = score.score;
    //     //         panneau.AfficherScore();
    //     //     }
    //     //     else // #tp4 luka si le score est plus petit que le score du joueur
    //     //     {
    //     //         Debug.LogWarning("creation panneau");
    //     //         PanneauJoueur panneau = Instantiate(_panneauJoueurPrefab, transform);
    //     //         panneau.classement = classement;
    //     //         panneau.nom = score.nom;
    //     //         panneau.score = score.score;
    //     //         panneau.AfficherScore();
    //     //     }
    //     // }
    //     for (int i = 0; i < donneesScores.Count; i++)
    //     {
    //         classement++;
    //         var score = donneesScores[i];

    //         // If the player's score has not been displayed yet and should be inserted
    //         if (!nouveauScoreEstAffiche && _donneesPerso.pointage > score.score)
    //         {
    //             nouveauScoreEstAffiche = true;
    //             PanneauJoueur panneauNouveau = Instantiate(_panneauJoueurPrefab, transform);
    //             panneauNouveau.scoreEstNouveau = true;
    //             panneauNouveau.classement = classement;
    //             panneauNouveau.score = _donneesPerso.pointage;
    //             panneauNouveau.nom = _donneesPerso.nom; // Assuming you have the player's name in _donneesPerso
    //             panneauNouveau.AfficherScore();

    //             classement++; // Increment classement for the next item
    //         }

    //         // Display the current score from the list
    //         PanneauJoueur panneau = Instantiate(_panneauJoueurPrefab, transform);
    //         panneau.classement = classement;
    //         panneau.nom = score.nom;
    //         panneau.score = score.score;
    //         panneau.AfficherScore();
    //         if (!nouveauScoreEstAffiche)
    //         {
    //             classement++;
    //             Debug.LogWarning("creation panneau");
    //             PanneauJoueur panneau = Instantiate(_panneauJoueurPrefab, transform);
    //             panneau.classement = classement;
    //             panneau.nom = score.nom;
    //             panneau.score = score.score;
    //             panneauNouveau.AfficherScore();
    //         }
    //     }
    // }
    void AfficherDonnees()
    {
        _sauvegarde.LireFichier();
        var donneesScores = _sauvegarde.ChercherListeScores();

        int classement = 0;

        for (int i = 0; i < donneesScores.Count; i++)
        {
            classement++;
            var score = donneesScores[i];

            // If the player's score has not been displayed yet and should be inserted
                Debug.Log(_donneesPerso.pointage);
            if (!nouveauScoreEstAffiche && _donneesPerso.pointage > score.score)
            {
                nouveauScoreEstAffiche = true;
                PanneauJoueur panneauNouveau = Instantiate(_panneauJoueurPrefab, transform);
                panneauNouveau.scoreEstNouveau = true;
                panneauNouveau.classement = classement;
                panneauNouveau.score = _donneesPerso.pointage;
                // panneauNouveau.nom = _donneesPerso.nom; // Assuming you have the player's name in _donneesPerso
                panneauNouveau.AfficherScore();

                classement++; // Increment classement for the next item
            }

            // Display the current score from the list
            PanneauJoueur panneau = Instantiate(_panneauJoueurPrefab, transform);
            panneau.classement = classement;
            panneau.nom = score.nom;
            panneau.score = score.score;
            panneau.AfficherScore();
        }

        // If the player's score is the lowest and was not inserted in the loop
        if (!nouveauScoreEstAffiche)
        {
            classement++;
            PanneauJoueur panneauNouveau = Instantiate(_panneauJoueurPrefab, transform);
            panneauNouveau.scoreEstNouveau = true;
            panneauNouveau.classement = classement;
            panneauNouveau.score = _donneesPerso.pointage;
            // panneauNouveau.nom = .nom; // Assuming you have the player's name in _donneesPerso
            panneauNouveau.AfficherScore();
        }
    }

}
