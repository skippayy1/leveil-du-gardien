using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MaNavigation", menuName = "Navigation", order = 52)]
/// <summary>
/// #tp3 Luka
/// Cette classe sert gerer le systeme de navigation du jeu
/// </summary>
public class SONavigation : ScriptableObject
{
    [SerializeField] SOPerso _donneesPerso; // #tp3 luka acces au scriptable object du perso

    /// <summary>
    /// #tp3 luka
    /// Cette methode permet change la scene a la scene de jeu
    /// </summary>
    public void Jouer()
    {
        _donneesPerso.InitialiserNiveau();
        AllerSceneSuivante();
    }

    public void AllerScenePointage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    /// <summary>
    /// #tp3 luka 
    /// Cette methode permet de sortir de la boutique
    /// </summary>
    public void SortirBoutique() // #tp3 luka
    {
        AllerScenePrecedente();
    }

    /// <summary>
    /// #tp3 luka 
    /// Cette methode permet rentrer dans la boutique 
    /// Cette methode permet de reinitialiser les valeurs du perso
    /// </summary>
    public void RentrerBoutique() // #tp3 luka
    {
        _donneesPerso.possedeCle = false; // #tp3 luka reinitialise la valeur de la cle lorsque le personnage rentre dans la boutique 
        _donneesPerso.niveau += 1; // #tp4 luka augmente le niveau du personnage
        _donneesPerso.InitialiserNiveau();
        AllerSceneSuivante();
    }
    /// <summary>
    /// #tp3 luka 
    /// Cette methode permet d'aller a la scene de menu
    /// </summary>
    public void AllerSceneMenu() // #tp3 luka
    {
        SceneManager.LoadScene(0);
        _donneesPerso.InitialiserJeu(); // #tp3 luka reinitialise les donnees du personnage
    }
    /// <summary>
    /// #tp3 luka 
    /// Cette methode permet d'aller a la scene suivante
    /// </summary>
    public void AllerSceneSuivante() // #tp3 luka
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    /// <summary>
    /// #tp3 luka 
    /// Cette methode permet d'aller a la scene precedente
    /// </summary>
    public void AllerScenePrecedente() // #tp3 luka
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void AllerSceneGenerique()
    {
        int indexDerniereScene = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(indexDerniereScene);
    }
}
