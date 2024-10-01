using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAttaque : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponentInParent<EnnemiBase>().EnleverVie();
        }
    }
}
