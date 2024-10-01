using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Classe responsable de la gestion de l'apparition aléatoire de tuiles dans un niveau.
/// Cette classe se trouve sur un objet enfant d'une salle qui à un composant Tilemap.
/// </summary>
public class CarteTuiles : MonoBehaviour
{
    [SerializeField, Range(0, 100)] int _chanceApparition;  // Taux de chance d'apparition des tuiles. Affiché avec un slider.

    Tilemap _tilemap;  // Tilemap associée à l'objet sur lequel se trouve cette classe.
    Niveau _niveau;  // Référence au niveau parent.

    /// <summary>
    /// Méthode appelée au réveil de l'objet. Elle configure les références et envoie les tuiles dans le niveau.
    /// </summary>
    void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        _niveau = GetComponentInParent<Niveau>();

        // Envoie des tuiles dans le niveau:
        EnvoyerTuiles();

        // Désactivation de l'objet:
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Méthode responsable d'envoyer les tuiles de la carte dans le niveau avec une probabilité définie par le taux de chance d'apparition.
    /// </summary>
    void EnvoyerTuiles()
    {
        // Calcul du décalage par rapport à la position de la carte:
        Vector3Int decalage = Vector3Int.FloorToInt(transform.position);

        // Génération d'un nombre aléatoire pour déterminer l'apparition des tuiles:
        int entierAlea = Random.Range(0, 101);

        // Vérification de la probabilité d'apparition:
        if (entierAlea > _chanceApparition) return;

        // Parcours de toutes les tuiles de la tilemap:
        BoundsInt bounds = _tilemap.cellBounds;
        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int pos = new(x, y);
                TileBase tuile = _tilemap.GetTile(pos);

                // Transfert de la tuile dans le niveau si elle existe:
                if (tuile != null)
                    _niveau.TransfererTuile(pos + decalage, tuile);
            }
        }
    }

    /// <summary>
    /// Méthode appelée lors de la validation des propriétés de l'objet.
    /// Elle ajuste l'alpha de la Tilemap en fonction du taux de chance d'apparition.
    /// </summary>
    void OnValidate() => GetComponent<Tilemap>().color = new Color(1, 1, 1, _chanceApparition / 100f);
}
