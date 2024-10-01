using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// #tp4 luka
/// cette classe permet de stocker les données du joueur
/// </summary>
[Serializable]
public class ListeDonneesJoueur
{
    public List<DonneesJoueur> listeScores;

    public ListeDonneesJoueur()
    {
        listeScores = new List<DonneesJoueur>();
    }


}

/// <summary>
/// #tp4 luka
/// cette classe permet de stocker les données du joueur
/// </summary>
[Serializable]
public class DonneesJoueur
{
    public string nom;
    public int score;

    public DonneesJoueur(string nom, int score)
    {
        this.nom = nom;
        this.score = score;

    }
}


/// <summary>
/// #tp4 luka
/// Ce scriptable object permet de lire et ecrire un json pour stocker les données de sauvegarde
/// </summary>
[CreateAssetMenu(fileName = "SOSauvegarde", menuName = "ScriptableObjects/SOSauvegarde", order = 1)]
public class SOSauvegarde : ScriptableObject
{
    // [SerializeField] SOPerso _donneesPerso;
    // [SerializeField] int _score = 100;
    // [SerializeField] string _nom = "Tim";
    // List<DonneesJoueur> _listeScores = new();
    [SerializeField] ListeDonneesJoueur _listeScores = new(); // #tp4 luka liste des scores
    public List<DonneesJoueur> listeScores => _listeScores.listeScores; // #tp4 luka accesseur pour la liste des scores

    /// <summary>
    /// #tp4 luka
    /// methode pour ajouter un resultat a la liste des scores
    /// </summary>
    public void AjouterResultat(string nom, int score)
    {
        listeScores.Add(new DonneesJoueur(nom, score));
        listeScores.Sort((a, b) => b.score.CompareTo(a.score));
        if (listeScores.Count > 5) listeScores.RemoveAt(listeScores.Count - 1);

        EcrireFichier();

    }
    /// <summary>
    /// #tp4 luka
    /// methode pour lire un fichier json  
    /// </summary>
    public void LireFichier()
    {

        // donner input d'infos
        string _fichierEtChemin = Application.persistentDataPath + "/sauvegarde.TIM";
        Debug.Log(_fichierEtChemin);

        if (File.Exists(_fichierEtChemin))
        {
            try
            {
                string contenu = File.ReadAllText(_fichierEtChemin);
                _listeScores = JsonUtility.FromJson<ListeDonneesJoueur>(contenu);

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
#endif
                Debug.Log(contenu);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Erreur dans la lecture du fichier: {ex.Message}");
            }
        }
        else
        {
            Debug.Log("Fichier inexistant");
            EcrireFichier();
            LireFichier();
        }
    }

    /// <summary>
    /// #tp4 luka
    /// methode pour chercher la liste des scores
    /// </summary>
    public List<DonneesJoueur> ChercherListeScores()
    {
        return _listeScores.listeScores;
    }

    /// <summary>
    /// #tp4 luka
    /// methode pour ecrire un fichier json
    /// </summary>
    public void EcrireFichier()
    {
        string _fichierEtChemin = Application.persistentDataPath + "/sauvegarde.TIM";
        string contenu = JsonUtility.ToJson(_listeScores, true);
        try
        {
            Debug.Log($"Tentative d'écriture du fichier : {_fichierEtChemin}");
            File.WriteAllText(_fichierEtChemin, contenu);
            Debug.Log("Fichier écrit avec succès.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Échec de l'écriture du fichier : {ex.Message}");
        }

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            try
            {
                SynchroniserWebGL();
                Debug.Log("Synchronisation WebGL réussie.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Échec de la synchronisation WebGL : {ex.Message}");
            }
        }
    }

    [DllImport("__Internal")]
    private static extern void SynchroniserWebGL();
}

