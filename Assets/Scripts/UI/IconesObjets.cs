using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// #tp3 luka
/// Cette classe permet de gerer les animations et les etats des icones des objets du jeu
/// </summary>
public class IconesObjets : MonoBehaviour
{
    [SerializeField] GameObject _iconePrefab; // Prefab pour l'icone
    [SerializeField] Transform _iconeParent; // parent des icones pour placer les icones a l'interieur de celle-ci
    [SerializeField] SOObjet[] _soObjet; // #tp4 luka Soobjet pour l'icone
    [SerializeField] SOPerso _donneesPerso; // #tp4 luka donnees du perso

    void Awake()
    {
        foreach (SOObjet objet in _donneesPerso.lesObjets)
        {
            if(objet.estAchete == true)
            {
                InstantierObjet(objet);
            }

        }
    }

    void InstantierObjet(SOObjet objet)
    {
            GameObject iconObject = Instantiate(_iconePrefab, _iconeParent);

            Image iconeImage = iconObject.GetComponent<Image>();

            // determiner le sprite de licone en utilisant le sprite de l'objet SO attache a ce gameObject
            if (iconeImage != null && _soObjet != null)
            {
                iconeImage.sprite = objet.sprite;
            }
            else
            {
                Debug.LogWarning("IconesObjets:Manque la composante image ou SOObjet.");
            }
    }
}