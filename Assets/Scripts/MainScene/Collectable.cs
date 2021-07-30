using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collectable : MonoBehaviour
{
    public static Action OnCoinCollected;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(!(OnCoinCollected is null))
                OnCoinCollected();

            Destroy(gameObject);
        }
    }
}
