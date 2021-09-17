using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class attached to each collectable on the game
public class Collectable : MonoBehaviour
{
    //Action delegate to be called when the collectable gets collected
    public static Action OnCollection;

    [Header("Collectable SFX")]
    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField] [Range(0f, 1f)]
    private float _volume;

    //Method to trigger collisions with the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!(OnCollection is null))
                OnCollection();

            AudioManager.Instance.PlayOneShot(_audioClip, _volume);
            Destroy(gameObject);
        }
    }
}
