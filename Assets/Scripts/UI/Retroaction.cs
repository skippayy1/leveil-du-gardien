using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// #tp3 Victor
/// Cette classe sert se retrouve son le modèle de rétroaction
/// Un fois instancié, le texte peut étre changer avec la référence à cette classe
/// </summary>
public class Retroaction : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _champ;

    public void ChangerTexte(string texte)
    {
        _champ.text = texte;
    }

    public void Detruire()
    {
        Destroy(gameObject);
    }
}
