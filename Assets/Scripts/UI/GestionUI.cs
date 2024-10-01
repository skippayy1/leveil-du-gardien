using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GestionUI : MonoBehaviour
{

    [SerializeField] TMP_Text _champPointage;
    [SerializeField] TMP_Text _champEngrenages;
    [SerializeField] TMP_Text _champNiveau;
    [SerializeField] SOPerso _donneesPerso;
    [SerializeField] SOObjet[] _donneesObjet;
    [SerializeField] BarreDeVie _barreDeVie;
    [SerializeField] GameObject _panneauObjets;
    [SerializeField] GameObject iconePrefab;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        MettreAJourInterface();
        _donneesPerso.evenementMiseAJour.AddListener(MettreAJourInterface);
    }

    public void MettreAJourInterface()
    {
        _champPointage.text = "Pointage: " + _donneesPerso.pointage.ToString("D6");
        _champEngrenages.text = "Engrenages: " + _donneesPerso.engrenages.ToString("D6");
        _champNiveau.text = "Niveau: " + _donneesPerso.niveau;
        // _barreDeVie.localScale = new Vector3(_donneesPerso.vie / 100f, 1, 1);
        _barreDeVie.ReduireBarre();
    }

    public void InstantierIcones()
    {
        foreach (SOObjet objet in _donneesObjet)
        {
            if (objet.estAchete)
            {
                GameObject iconeGO = Instantiate(iconePrefab, _panneauObjets.transform);
                iconeGO.name = "Icone" + objet.nom;

                SpriteRenderer spriteRenderer = iconeGO.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = objet.sprite;
                }
            }
        }
    }
}
