using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using Cinemachine;

/// <summary>
/// Gère la génération aléatoire de salles et la configuration du contour du niveau.
/// Classe présente sur l'objet de jeu du niveau.
/// </summary>
public class Niveau : MonoBehaviour
{
    [SerializeField] Tilemap _tilemap;  // Tilemap utilisée pour afficher le niveau complet.
    [SerializeField] Vector2Int _nbSalles = new(2, 3);  // Nombre de salles dans le niveau (en termes de largeur et de hauteur).
    [SerializeField] Salle[] _sallesModele;  // Modèles de salles disponibles pour le niveau.
    [SerializeField] TileBase _tuileContour;  // Tuile utilisée pour définir le contour du niveau.
    [SerializeField] SOPerso _donneesPerso; // #tp4 luka acces au scriptable object du perso

    #region #tp3 Victor
    [Header("Objets à placer sur les repères")]
    [SerializeField] GameObject _cleModele;
    [SerializeField] GameObject _porteModele;
    [SerializeField] GameObject _activateurModele;
    [SerializeField] GameObject _portailModele;

    [Header("Objets à placer au hasard")]
    [SerializeField] GameObject[] _joyauxModeles;
    [SerializeField] int _nbJoyauxParSalle = 20;
    [Space]
    [SerializeField] GameObject[] _bonusModeles;
    [SerializeField] int _nbBonusParSalle = 20;

    List<Vector2Int> _lesPosLibres = new();  // Liste des positions des tuiles libres dans le niveau.
    List<Vector2Int> _lesPosSurReperes = new();  // Liste des positions des repères utilisés.
    List<Vector2Int> _lesSallesRepereLibre = new();  // Liste des positions des salles disponibles pour le placement des objets.
    #endregion

    #region #tp4 Victor
    [Header("Confineur de la caméra")]
    [SerializeField] PolygonCollider2D _colliderConfineurCamera;  // Confineur de la caméra
    [SerializeField] CinemachineConfiner2D _confineurCamera;  // Confineur de la caméra
    #endregion

    [Header("Personnage")]
    [SerializeField] Perso _perso; // #synthese luka acces au script du perso
    public Perso perso => _perso; // #synthese luka getter du perso

    UnityEvent _evenementActivationBonus = new UnityEvent(); // tp3 luka evenement pour lactivateur
    public UnityEvent evenementActivationBonus => _evenementActivationBonus; // accesseur en lecture pour l'activateur

    public void InvokerEvenementActivateur()
    {
        evenementActivationBonus.Invoke();
        Debug.Log("appel de func pour invoker l'evenement");
    }

    // #tp3 luka rend la classe niveau un singleton
    private static Niveau _instance;
    public static Niveau instance => _instance;

    /// <summary>
    /// Méthode appelée au réveil de l'objet. Elle génère le niveau en plaçant aléatoirement des salles et en définissant le contour.
    /// </summary>
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        #region #tp3 Victor
        if (_donneesPerso.niveau > 1) AgrandirNiveau(); // #tp4 luka Agrandit le niveau en fonction du niveau du personnage.
        InitialiserSallesRepereLibre();  // Initialise les salles disponibles pour le placement des objets spéciaux.
        CreerNiveau();  // Méthode qui génère le niveau en plaçant aléatoirement des salles et en définissant le contour.
        TrouverPosLibres();  // Trouve les positions libres dans le niveau pour le placement d'objets aléatoires.
        PlacerObjetsAleatoire("Joyaux", _nbJoyauxParSalle, _joyauxModeles);  // Place aléatoirement des joyaux dans le niveau.
        PlacerObjetsAleatoire("Bonus", _nbBonusParSalle, _bonusModeles);  // Place aléatoirement des bonus dans le niveau.
        #endregion
    }

    private void CreerNiveau()
    {
        // Calcul de la taille d'une salle avec une seule bordure:
        Vector2Int tailleSalleAvecUneBordure = Salle.tailleAvecBordures - Vector2Int.one;

        #region #tp3 Victor
        Vector2Int placementCle = ObtenirUneSalleRepereLibre(true);  // Obtient une position de salle libre pour placer la clé.
        Vector2Int placementPorte = ObtenirSalleOpposee(placementCle);  // Obtient la position opposée à la clé pour placer la porte.
        Vector2Int placementActivateur = ObtenirUneSalleRepereLibre(false);  // Obtient une position de salle libre pour placer l'activateur.
        Vector2Int placementPortail = ObtenirUneSalleRepereLibre(false);  // Obtient une position de salle libre pour placer le portail.
        #endregion

        // Placement aléatoire des salles dans le niveau:
        for (int y = 0; y < _nbSalles.y; y++)
        {
            for (int x = 0; x < _nbSalles.x; x++)
            {
                // Sélection aléatoire d'un modèle de salle:
                Salle salleAlea = _sallesModele[Random.Range(0, _sallesModele.Length)];
                salleAlea.estAuSol = y == 0;  // #tp4 Victor Définit si la salle est au sol ou non.

                // Calcul de la position de la salle dans le niveau:
                Vector2 pos = new(x * tailleSalleAvecUneBordure.x, y * tailleSalleAvecUneBordure.y);

                // Instanciation de la salle et configuration de son nom:
                Salle salle = Instantiate(salleAlea, pos, Quaternion.identity, transform);
                salle.name = $"Salle_{x}_{y}";

                #region #tp3 Victor
                // Place les objets spéciaux (clé, porte, activateur, portail) sur les repères des salles correspondantes.
                Vector2Int placementSalle = new(x, y);
                if (placementCle == placementSalle)
                    _lesPosSurReperes.Add(salle.PlacerSurRepere(_cleModele));
                if (placementPorte == placementSalle)
                    _lesPosSurReperes.Add(salle.PlacerSurRepere(_porteModele));
                if (placementActivateur == placementSalle)
                    _lesPosSurReperes.Add(salle.PlacerSurRepere(_activateurModele));
                #endregion
                // #synthese Victor
                if (placementPortail == placementSalle)
                    _lesPosSurReperes.Add(salle.PlacerSurRepere(_portailModele));
            }
            DefinirConfineurCamera(tailleSalleAvecUneBordure);  // #tp4 Victor Définit le contour de la caméra en fonction du niveau.
        }

        /// <summary>
        /// Définit le contour de la caméra en fonction du niveau.
        /// #tp4 Victor
        /// </summary>
        /// <param name="tailleSalleAvecUneBordure">Taille d'une salle avec une seule bordure.</param>
        void DefinirConfineurCamera(Vector2Int tailleSalleAvecUneBordure)
        {
            Vector2[] points = new Vector2[4];
            Vector2 coinMin = Vector2Int.zero;
            Vector2 coinMax = new(_nbSalles.x * tailleSalleAvecUneBordure.x + 1, _nbSalles.y * tailleSalleAvecUneBordure.y + 1);
            points[0] = coinMin;
            points[1] = new Vector2(coinMax.x, coinMin.y);
            points[2] = coinMax;
            points[3] = new Vector2(coinMin.x, coinMax.y);
            _colliderConfineurCamera.SetPath(0, points);
            _colliderConfineurCamera.transform.position = (Vector3Int)(-Salle.tailleAvecBordures / 2);
            _confineurCamera.InvalidateCache();
        }

        // Calcul de la taille totale du niveau:
        Vector2Int tailleNiveau = new Vector2Int(_nbSalles.x, _nbSalles.y) * tailleSalleAvecUneBordure;

        // Définition des limites du niveau:
        Vector2Int min = Vector2Int.zero - Salle.tailleAvecBordures / 2;
        Vector2Int max = min + tailleNiveau;

        // Placement des tuiles de contour autour du niveau:
        for (int y = min.y; y <= max.y; y++)
        {
            for (int x = min.x; x <= max.x; x++)
            {
                Vector3Int pos = new(x, y);

                // Vérification si la tuile est sur le bord du niveau, et si oui,
                // place la tuile de contour aux coordonnés en question:
                if (x == min.x || x == max.x || y == min.y || y == max.y)
                    _tilemap.SetTile(pos, _tuileContour);
            }
        }
    }

    /// <summary>
    /// Méthode permettant de transférer une tuile à une position spécifiée dans la Tilemap.
    /// Cette méthode est appelé dans la classe salle.
    /// </summary>
    /// <param name="pos">Position où la tuile doit être transférée.</param>
    /// <param name="tuile">Tuile à transférer.</param>
    public void TransfererTuile(Vector3Int pos, TileBase tuile) => _tilemap.SetTile(pos, tuile);

    #region #tp3 Victor
    /// <summary>
    /// Initialise les salles disponibles pour le placement des objets spéciaux.
    /// </summary>
    void InitialiserSallesRepereLibre()
    {
        for (int y = 0; y < _nbSalles.y; y++)
        {
            for (int x = 0; x < _nbSalles.x; x++)
            {
                _lesSallesRepereLibre.Add(new(x, y));
            }
        }
    }

    /// <summary>
    /// Obtient la position opposée à une salle donnée pour le placement d'un objet spécifique.
    /// </summary>
    /// <param name="salle">Position de la salle actuelle.</param>
    /// <returns>La position opposée à la salle donnée.</returns>
    Vector2Int ObtenirSalleOpposee(Vector2Int salle)
    {
        Vector2Int salleOpposee = _nbSalles - salle - Vector2Int.one;
        _lesSallesRepereLibre.Remove(salleOpposee);
        return salleOpposee;
    }

    /// <summary>
    /// Obtient une salle libre pour le placement d'un objet spécifique.
    /// </summary>
    /// <param name="estEnBordure">Indique si la salle doit être en bordure.</param>
    /// <returns>Une salle libre pour le placement d'un objet spécifique.</returns>
    Vector2Int ObtenirUneSalleRepereLibre(bool estEnBordure)
    {
        List<Vector2Int> lesSallesRepereLibre = estEnBordure ?
            ObtenirLesSallesEnBordure(_lesSallesRepereLibre) : _lesSallesRepereLibre;
        int indexPosLibre = Random.Range(0, lesSallesRepereLibre.Count);
        Vector2Int salleRepereLibre = lesSallesRepereLibre[indexPosLibre];
        _lesSallesRepereLibre.Remove(salleRepereLibre);
        return salleRepereLibre;
    }

    /// <summary>
    /// Obtient les salles en bordure parmi une liste de salles disponibles.
    /// </summary>
    /// <param name="lesSallesRepereLibre">Liste des salles disponibles.</param>
    /// <returns>Les salles en bordure parmi la liste des salles disponibles.</returns>
    List<Vector2Int> ObtenirLesSallesEnBordure(List<Vector2Int> lesSallesRepereLibre)
    {
        List<Vector2Int> lesSallesEnBordure = new();
        foreach (Vector2Int posSalle in lesSallesRepereLibre)
        {
            if (posSalle.x == 0 || posSalle.x == _nbSalles.x - 1 || posSalle.y == 0 || posSalle.y == _nbSalles.y - 1)
            {
                lesSallesEnBordure.Add(posSalle);
            }
        }
        return lesSallesEnBordure;
    }

    /// <summary>
    /// Place aléatoirement des objets spéciaux dans le niveau.
    /// </summary>
    /// <param name="nomConteneur">Nom du conteneur des objets spéciaux.</param>
    /// <param name="nbObjetsParSalle">Nombre d'objets spéciaux à placer par salle.</param>
    /// <param name="modeles">Modèles des objets spéciaux à placer.</param>
    void PlacerObjetsAleatoire(string nomConteneur, int nbObjetsParSalle, GameObject[] modeles)
    {
        Transform conteneur = new GameObject(nomConteneur).transform;
        conteneur.parent = transform;
        int nbObjets = nbObjetsParSalle * _nbSalles.x * _nbSalles.y;
        for (int i = 0; i < nbObjets; i++)
        {
            Vector2Int pos = ObtenirUnePosLibre();
            Vector3 pos3 = (Vector3)(Vector2)pos + _tilemap.transform.position + _tilemap.tileAnchor;
            int indexModelAlea = Random.Range(0, modeles.Length);
            Instantiate(modeles[indexModelAlea], pos3, Quaternion.identity, conteneur);
            if (_lesPosLibres.Count == 0) { Debug.LogWarning("Aucun espace libre"); break; }
        }
    }

    /// <summary>
    /// Libère une position dans le niveau.
    /// </summary>
    /// <param name="posPrecise">Position précise à libérer.</param>
    public void LibererUnePos(Vector3 posPrecise)
    {
        Vector2Int pos = Vector2Int.FloorToInt(posPrecise - _tilemap.transform.position);
        _lesPosLibres.Add(pos);
    }

    /// <summary>
    /// Obtient une position libre dans le niveau.
    /// </summary>
    /// <returns>Une position libre dans le niveau.</returns>
    private Vector2Int ObtenirUnePosLibre()
    {
        int indexPosLibre = Random.Range(0, _lesPosLibres.Count);
        Vector2Int pos = _lesPosLibres[indexPosLibre];
        _lesPosLibres.RemoveAt(indexPosLibre);
        return pos;
    }

    /// <summary>
    /// Trouve les positions libres dans le niveau pour le placement d'objets aléatoires.
    /// </summary>
    void TrouverPosLibres()
    {
        BoundsInt bornes = _tilemap.cellBounds;
        for (int y = bornes.min.y; y < bornes.max.y; y++)
        {
            for (int x = bornes.min.x; x < bornes.max.x; x++)
            {
                Vector3Int posTuile = new(x, y, 0);
                TileBase tuile = _tilemap.GetTile(posTuile);
                if (tuile == null) _lesPosLibres.Add(new(x, y));
            }
        }
        foreach (Vector2Int pos in _lesPosSurReperes)
        {
            _lesPosLibres.Remove(pos);
        }
    }
    #endregion
    /// <summary>
    /// #tp4 luka
    /// cette methode permet d'agrandir le niveau en fonction du niveau du personnage
    /// </summary>
    void AgrandirNiveau()
    {
        _nbSalles.x += 1;

        if (_donneesPerso.niveau == 4)
        {
            _nbSalles.y += 1;
        }

        if (_donneesPerso.niveau % 10 == 0) // #tp4 luka si le niveau est un multiple de 10
        {
            _nbSalles.x -= 2;
        }
    }
}