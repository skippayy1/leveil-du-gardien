using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

/// <summary>
/// Classe du portail
/// Gère le portail de téléportation avec la transition
/// #synthese Victor
/// </summary>
public class Portail : MonoBehaviour
{
    [SerializeField] SOPairePortails _pairePortails; // Référence de la paire de portails
    [SerializeField] SOPerso _donneesPerso; // Référence des données du personnage
    [SerializeField] CinemachineVirtualCamera _camera; // Caméra du portail
    [SerializeField] float _tempsRecharge = 1f; // Temps de recharge du portail

    int _indexPortailCible; // Index du portail cible
    Collider2D _collider; // Collider du portail

    void Start()
    {
        _indexPortailCible = _pairePortails.DonnerReference(this);
        _collider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si le joueur entre dans le portail, on appelle la fonction de transition
        // La fonction prend l'instance du portail en paramètre pour appeler la fonction TeleporterDepuis après la transition
        if (other.gameObject.CompareTag("Player")) Transition.Instance.FaireTransition(1, 0, this);
    }

    /// <summary>
    /// Étapes pour téléporter le joueur depuis le portail
    /// </summary>
    public void TeleporterDepuis()
    {
        _camera?.gameObject.SetActive(false);
        _pairePortails.ObtenirPortail(_indexPortailCible).TeleporterVers();
    }

    /// <summary>
    /// Étapes pour téléporter le joueur vers le portail
    /// </summary>
    public void TeleporterVers()
    {
        _camera?.gameObject.SetActive(true);
        _donneesPerso.perso.transform.position = transform.position;
        Transition.Instance.FaireTransition(0, 1);
        StartCoroutine(CoroutActiverCollider());
    }

    /// <summary>
    /// Coroutine pour activer le collider après un temps de recharge
    /// </summary>
    IEnumerator CoroutActiverCollider()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(_tempsRecharge);
        _collider.enabled = true;
    }
}
