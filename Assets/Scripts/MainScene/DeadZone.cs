using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class attached to the collider below the scene to trigger player deaths
public class DeadZone : MonoBehaviour
{
    //Action delegate to be called when player dies
    public static Action OnPlayerFall;
    
    [Header ("Death SFX")]
    [SerializeField]
    private AudioClip _fallSound;
    [SerializeField] [Range(0f, 1f)]
    private float _volume;

    //Method to trigger collisions with the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!(OnPlayerFall is null))
            {
                OnPlayerFall();
                AudioManager.Instance.PlayOneShot(_fallSound, _volume);
            }
        }
    }
}
