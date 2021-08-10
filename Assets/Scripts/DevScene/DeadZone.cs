using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DevScene
{
    public class DeadZone : MonoBehaviour
    {
        public static Action OnPlayerFall;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!(OnPlayerFall is null))
                    OnPlayerFall();
            }
        }
    }
}