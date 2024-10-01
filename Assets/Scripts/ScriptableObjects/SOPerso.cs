using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "LePerso", menuName = "Perso", order = 51)]
/// <summary>
/// #tp3 luka
/// Cette classe permet de gerer les valeurs du personnage a linterieur d'un scriptable object
/// </summary>
public class SOPerso : ScriptableObject
{
    [SerializeField] AudioClip _sonDegats; // Son lorque le perso prend des dégats
    [Header("Les valeurs actuelles:")]
    [SerializeField, Range(0, 200)] int _engrenages = 0; // #tp3 luka slider pour les engrenages actuels de la partie en cours (courant monetaire du jeu)
    [SerializeField] int _pointage = 0; // #tp3 luka pointage actuel de la partie en cours
    [SerializeField] bool _possedeCle; // #tp3 luka verifie si le joueur possede la cle
    [SerializeField] bool _peutEsquiver; // #tp3 luka bool pour verifier si le perso possede la ceinture pour esquiver
    [SerializeField] bool _peutDoubleSaut = false; // #tp3 luka bool veridie si le double est achete
    [SerializeField] bool _possedeHallebardeSpecial = false; // #tp3 Victor Est-ce que la hallebarde spécial est acheté
    [SerializeField] int _niveau = 1; // #tp4 luka niveau actuel du personnage
    [SerializeField] int _vie = 100; // #synthese luka vie actuelle du personnage
    [Header("Les valeurs initiales:")]
    [SerializeField, Range(0, 200)] int _engrenagesIni = 0; // #tp3 luka valeure initiale du courant monetaire (utilisee lors de l'initialisation du jeu)
    [SerializeField] int _pointageIni = 0; // #tp3 luka valeure initiale du pointage (utilisee lors de l'initialisation du jeu)
    [SerializeField] bool _possedeCleIni = false; // #tp3 luka verifie si le joueur possede la cle
    [SerializeField] bool _peutEsquiverIni = false; // #tp3 luka valeure initiale de verification de la possession de la ceinture
    [SerializeField] bool _peutDoubleSautIni = false; // #tp3 luka valeure initiale de verification si le personnage peut double sauter
    [SerializeField] bool _possedeHallebardeSpecialInit = false; // #tp3 Victor Est-ce que la hallebarde spécial est acheté
    [SerializeField] int _niveauIni = 1; // #tp4 luka niveau initial du personnage
    [SerializeField] int _vieIni = 100; // #synthese luka vie actuelle du personnage

    Perso _perso;
    public Perso perso
    {   // #tp3 luka accesseur et mutateur pour acceder au personnage
        get => _perso;
        set { _perso = value; }
    }

    public int engrenages
    { // #tp3 luka accesseur et mutateur pour les engrenages
        get => _engrenages;
        set
        {
            _engrenages = Mathf.Clamp(value, 0, int.MaxValue);
            AjouterPointage(value / 3);
            _evenementMiseAJour.Invoke(); // #tp3 luka invocation de l'evenement de mise a jour
        }
    }

    public int vie
    { // #synthese luka accesseur et mutateur pour la vie
        get => _vie;
        set{
            GestSons.instance.JouerSon(_sonDegats, 1f);
            _vie = value;
        _evenementMiseAJour.Invoke(); // #synthese luka invocation de l'evenement de mise a jour    
        } 
    }

    public int pointage
    { // #tp3 luka accesseur et mutateur pour le pointage
        get => _pointage;
        set
        {
            _pointage = Mathf.Clamp(value, 1, int.MaxValue);
            _evenementMiseAJour.Invoke(); // #tp3 luka invocation de l'evenement de mise a jour
        }
    }

    public bool possedeCle
    { // #tp3 luka accesseur et mutateur pour verifier la possession de la cle
        get => _possedeCle;
        set => _possedeCle = value;
    }

    public int niveau
    { // #tp4 luka accesseur et mutateur pour le niveau
        get => _niveau;
        set => _niveau = value;
    }

    public bool peutEsquiver
    { // #tp3 luka accesseur et mutateur pour verifier la possession de la ceinture
        get => _peutEsquiver;
        set => _peutEsquiver = value;
    }

    public bool peutDoubleSaut
    {
        get => _peutDoubleSaut;
        set => _peutDoubleSaut = value;
    }

    UnityEvent _evenementMiseAJour = new UnityEvent(); // #tp3 luka creation de l'evenement de mise a jour
    public UnityEvent evenementMiseAJour => _evenementMiseAJour; // #tp3 luka creation du getter pour l'evenement de mise a jour
    [SerializeField] List<SOObjet> _lesObjets = new List<SOObjet>(); // liste d'objets que possede le personnage (achetes dans la boutique)
    public List<SOObjet> lesObjets => _lesObjets; // #tp3 luka getter pour la liste d'objets

    /// <summary>
    /// #tp3 luka
    /// methode pour reinitialiser les donnes du personnage au debut du jeu(engrenages et pointage)
    /// </summary>
    public void InitialiserJeu()
    {
        Debug.Log("Jeu initialiser");
        _engrenages = _engrenagesIni;
        _pointage = _pointageIni;
        _niveau = _niveauIni;
        _vie = _vieIni;
    }

    /// <summary>
    /// #tp3 luka
    /// methode pour reinitialiser les donnes du personnage au debut du niveau(cle et objets)
    /// </summary>
    public void InitialiserNiveau()
    {
        _possedeCle = _possedeCleIni;
        _peutEsquiver = _peutEsquiverIni;
        _peutDoubleSaut = _peutDoubleSautIni;
        _possedeHallebardeSpecial = _possedeHallebardeSpecialInit;
        _vie = _vieIni;
        _lesObjets.Clear();
    }

    public void ReinitialiserObjets() // #tp3 luka
    {
        _lesObjets.Clear();
    }

    /// <summary>
    /// #tp3 luka 
    /// cette methode permet de reduire le nombre d'engrenages du personnage lorsqu'il
    /// achete un objet dans la boutique
    /// </summary>
    /// <param name="donneesObjet"></param>
    public void Acheter(SOObjet donneesObjet)
    {
        if (engrenages >= donneesObjet.prix)
        {
            engrenages -= donneesObjet.prix;
            _lesObjets.Add(donneesObjet);
            donneesObjet.estAchete = true;
            AfficherInventaire();
            ActiverPouvoirs();
        }
    }
    /// <summary>
    /// #tp3 luka
    /// Active les pouvoirs du personnage qui ont ete achete dans la boutique
    /// </summary>
    public void ActiverPouvoirs()
    {
        foreach (var objet in _lesObjets)
        {
            if (objet.nom == "Ceinture")
            {
                _peutEsquiver = true;
            }
            else if (objet.nom == "Masque")
            {
                _peutDoubleSaut = true;
            }
            else if (objet.nom == "Hallebarde")
            {
                _possedeHallebardeSpecial = true;
            }
        }
    }

    /// <summary>
    /// #tp3 luka
    /// affiche l'inventaire du personnage dans la console afin de verifier que la boutique fonctionne correctement
    /// </summary>
    void AfficherInventaire()
    {
        string inventaire = "";
        foreach (SOObjet objet in _lesObjets)
        {
            if (inventaire != "") inventaire += ", ";
            inventaire += objet.nom; // ajoute une virgule et le nom de l'objet dans la liste
        }
        Debug.Log("Inventaire du perso: " + inventaire);
    }
    /// <summary>
    /// #tp3 luka
    /// methode pour ajouter du pointage 
    /// </summary>
    /// <param name="ajout"></param>
    public void AjouterPointage(int ajout)
    {
        pointage += ajout;
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate() // #tp3 luka
    {
        _evenementMiseAJour.Invoke(); // #tp3 luka invocation de l'evenement de mise a jour
    }
}
