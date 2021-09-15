using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeadZone : MonoBehaviour
{
    public static Action OnPlayerFall;
    [SerializeField]
    private AudioClip _fallSound;
    [SerializeField] [Range(0f, 1f)]
    private float _volume;

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
