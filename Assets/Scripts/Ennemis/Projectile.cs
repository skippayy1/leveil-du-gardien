using System.Collections;
using UnityEngine;
/// <summary>
/// #synthese luka
/// cette classe sert a gerer les projectiles
public class Projectile : MonoBehaviour
{
    [SerializeField] float _vitesse = 5f; // #synthese luka vitesse du projectile
    [SerializeField] float _dureeProjectile = 5f; // #synthese luka duree de vie du projectile
    [SerializeField] SOPerso _donneesPerso; // #synthese luka scriptable object du personnage
    [SerializeField] int _degats = 5; // #synthese luka degats du projectile
    private Rigidbody2D _rb; // #synthese luka rigidbody du projectile

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.up * _vitesse;
        Destroy(gameObject, _dureeProjectile); // #synthese luka detruire le projectile apres un certain temps
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Perso _scriptPerso = other.gameObject.GetComponent<Perso>();
            if (_scriptPerso != null && _scriptPerso.estEnBlock && _scriptPerso.estEnParry)
            {
                // _rb.velocity = transform.up * -_vitesse;
                StartCoroutine(CourbeParry());
            }
            else if (_scriptPerso != null && _scriptPerso.estEnBlock && !_scriptPerso.estEnParry)
            {
                _donneesPerso.vie -= _degats / 2; // #synthese luka enlever de la vie au joueur
                Destroy(gameObject);
            }
            else
            {
                _donneesPerso.vie -= _degats; // #synthese luka enlever de la vie au joueur
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// #synthese luka
    /// cette coroutine sert a faire une courbe lors d'un parry
    /// </summary>
    IEnumerator CourbeParry()
    {
        Vector2 targetDirection = transform.up * -1f;
        float curveTime = 1f; // Adjust this to change how long the curve takes
        float curveSpeed = 1f; // Adjust this to change the speed of the curve

        for (float t = 0; t < curveTime; t += Time.deltaTime)
        {
            // Calculate the current direction
            Vector2 currentDirection = Vector2.Lerp(_rb.velocity.normalized, targetDirection, t * curveSpeed);

            // Set the velocity
            _rb.velocity = currentDirection * _vitesse;

            yield return null;
        }

        // Ensure the final direction is the target direction
        _rb.velocity = targetDirection * _vitesse;
    }

}