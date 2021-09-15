using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collectable : MonoBehaviour
{
    public static Action OnCollection;
    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField] [Range(0f, 1f)]
    private float _volume;

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
