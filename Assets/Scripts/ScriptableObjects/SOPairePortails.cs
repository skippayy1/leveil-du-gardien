using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PairePortails", menuName = "ScriptableObjects/SOPairePortails")]
/// <summary>
/// Permet de référencer les deux portails d'une paire
/// Donner la référence de cet objet à chaque portail de la paire
/// #synthese Victor
/// </summary>
public class SOPairePortails : ScriptableObject
{
    [SerializeField] Portail _portail1;
    [SerializeField] Portail _portail2;

    /// <summary>
    /// Fonction pour donner la référence d'un portail
    /// </summary>
    /// <param name="portail">
    /// Le portail à référencer
    /// </param>
    /// <returns>
    /// L'index de l'autre portail,
    /// 0 si les deux portails sont déjà définis
    /// </returns>
    public int DonnerReference(Portail portail)
    {
        if (_portail1 == null) 
        {
            _portail1 = portail;
            return 2;
        }
        else if (_portail2 == null)
        {
           _portail2 = portail;
           return 1; 
        } 
        else Debug.LogError("Les deux portails sont déjà définis");
        return 0;
    }

    /// <summary>
    /// Fonction pour obtenir un portail
    /// </summary>
    /// <param name="numero">
    /// Le numéro du portail à obtenir
    /// </param>
    public Portail ObtenirPortail(int numero) => numero == 1 ? _portail1 : numero == 2 ? _portail2 : null;
}
