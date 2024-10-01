using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe de gestion des sons
/// #tp4 Victor
/// </summary>
public class GestSons : MonoBehaviour
{
    AudioSource _sourceAudio; // #tp4 Victor Source audio
    float _variaPitch = 0.1f; // #tp4 Victor Variation de pitch

    static GestSons _instance; // #tp4 Victor Singleton
    public static GestSons instance => _instance;

    void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
        _sourceAudio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Joue le clip sonore avec le volume spécifié
    /// #tp4 Victor
    /// </summary>
    public void JouerSon(AudioClip clip, float volume)
    {
        _sourceAudio.pitch = Random.Range(1 - _variaPitch, 1 + _variaPitch);
        _sourceAudio.PlayOneShot(clip, volume);
    }
}
