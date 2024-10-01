using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui controle la detection des collisions
/// Les classes qui en héritent peuvent utiliser la valeur de "_estEnCollision;"
/// Un gizmos qui montre la zone de collision s'affiche
/// Auteurs du code: Luka Munger et Victor Aubry
/// Auteur des commentaires: Victor Aubry
/// </summary>
public class DetectionCollisions : MonoBehaviour
{
    [Header("Paramètres de la collision")]
    //Décalage de la zone de collision par rapport au centre du GameObject:
    [SerializeField] Vector2 _decalage = new(0, -0.9f);
    //Grandeur de la zone de collision (indépendante de la grandeur du GameObject):
    [SerializeField] Vector2 _grandeur = new(0.4f, 0.2f);
    //Masque de collision
    [SerializeField] LayerMask _layerMask;

    protected bool _estEnCollision; //Détermine s'il y a une collision (accessible pour l'enfant)

    /// <summary>
    /// Additionne la position du GameObjet et le décalage
    /// </summary>
    /// <returns>La position décalée</returns>
    Vector2 PositionDecale() => (Vector2)transform.position + _decalage;

    virtual protected void FixedUpdate() => DetecterCollision();

    /// <summary>
    /// Détection s'il y a une collision et donne la valeur au booléen
    /// </summary>
    void DetecterCollision() => _estEnCollision = Physics2D.OverlapBox(PositionDecale(), _grandeur, 0, _layerMask);

    void OnDrawGizmos()
    {
        //Affiche le gizmos de la zone de collision
        //En vert s'il y a une collision, en rouge sinon
        if (!Application.isPlaying) DetecterCollision();
        Gizmos.color = _estEnCollision? Color.green : Color.red;
        Gizmos.DrawCube(PositionDecale(), _grandeur);
    }
}