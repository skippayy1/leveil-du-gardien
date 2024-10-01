using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe de gestion du fond de la salle
/// #tp3 Victor
/// </summary>
public class Fond : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] _tSRAvantPlan; // #tp3 Victor Tableau des sprites de l'avant-plan
    [SerializeField] SpriteRenderer[] _tSRArrierePlan; // #tp3 Victor Tableau des sprites de l'arrière-plan

    float _decalage; // #tp3 Victor Décalage du fond
    public float decalage { get =>_decalage; set { _decalage = value; } }

    Salle _salle; // #tp3 Victor Référence à la salle parente

    void Start()
    {
        _salle = transform.parent.GetComponent<Salle>();
        if(!_salle.estAuSol) foreach(SpriteRenderer spriteRenderer in _tSRAvantPlan) spriteRenderer.gameObject.SetActive(false);
    }

    void Update()
    {
        if(_salle.estAuSol) BougerPlans(_tSRAvantPlan);
        BougerPlans(_tSRArrierePlan);
    }

    /// <summary>
    /// Bouge les plans en fonction du décalage de la caméra
    /// #tp3 Victor
    /// </summary>
    void BougerPlans(SpriteRenderer[] spriteRenderers)
    {
        foreach(SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sharedMaterial.SetVector("_Decalage", Camera.main.transform.position);
        }
    }
}
