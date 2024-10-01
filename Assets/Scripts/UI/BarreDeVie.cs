using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// #tp4 luka
/// cette classe permet de gérer la barre de vie du joueur
/// </summary>
public class BarreDeVie : MonoBehaviour
{
    [SerializeField] SONavigation _soNavigation; // #tp4 luka scriptable object de navigation
    [SerializeField] SOPerso _soPerso; // #synthese luka scriptable object du personnage
    [SerializeField] Slider _slider; // #synthese luka slider de la barre de vie
    public static BarreDeVie instance; // #tp4 luka instance de la barre de vie


    void Start()
    {
        _slider.value = _soPerso.vie; // #synthese luka valeur du slider
        _soPerso.evenementMiseAJour.AddListener(ReduireBarre); // #tp4 luka ajout d'un ecouteur sur l'evenement de mise a jour de la barre de vie
        if (instance == null) // #tp4 luka si l'instance est nulle
        {
            instance = this; // #tp4 luka l'instance est egale a cette instance
        }
        else
        {
            Destroy(gameObject); // #tp4 luka detruire l'objet (il y a deja une instance de la barre de vie)
        }
    }
    /// <summary>
    /// #tp4 luka
    /// cette fonction permet de reduire la barre de vie du joueur
    /// </summary>
    public void ReduireBarre()
    {
        _slider.value = _soPerso.vie; // #synthese luka valeur du slider

        if (_soPerso.vie <= 0) // #synthese luka si la vie du joueur est inferieure ou egale a 0
        {
            _soNavigation.AllerScenePointage(); // #synthese luka changer de page pour la page de game over
        }
    }
}
