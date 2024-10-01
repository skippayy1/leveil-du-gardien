using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe représentant une salle dans le jeu. Les salles sont des éléments modélisant des espaces rectangulaires avec des bordures.
/// </summary>
public class Salle : MonoBehaviour
{
    // Taille d'une salle avec bordures
    static Vector2Int _tailleAvecBordures = new Vector2Int(32, 24);
    public static Vector2Int tailleAvecBordures => _tailleAvecBordures;

    [SerializeField] Transform _repere; //#tp3 Victor

    [SerializeField] bool _estAuSol;
    public bool estAuSol { get => _estAuSol; set { _estAuSol = value; } }

    void OnDrawGizmos()
    {
        // Couleur utilisée pour les gizmos:
        Gizmos.color = Color.red;

        // Dessin d'un cube en mode fil de fer pour représenter les limites de la salle:
        Gizmos.DrawWireCube(transform.position, new Vector3(_tailleAvecBordures.x, _tailleAvecBordures.y, 0));
    }

    #region #tp3 Victor
    /// <summary>
    /// Place un modèle d'objet sur le repère de la salle.
    /// </summary>
    /// <param name="modele">Le modèle d'objet à placer.</param>
    /// <returns>Les coordonnées de la position où l'objet a été placé.</returns>
    public Vector2Int PlacerSurRepere(GameObject modele)
    {
        // Obtient la position du repère de la salle:
        Vector3 pos = _repere.position;

        // Instancie le modèle d'objet à la position du repère avec une rotation identité:
        Instantiate(modele, pos, Quaternion.identity);

        // Retourne les coordonnées de la position où l'objet a été placé:
        return Vector2Int.FloorToInt(pos);
    }
    #endregion
}
