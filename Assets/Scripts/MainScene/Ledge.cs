using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    [SerializeField]
    private Transform _ledgeGrabbedPos, _playerStandPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LedgeGrabChecker"))
        {
            if(other.transform.parent.TryGetComponent(out Player player))
            {
                player.GrabLedge(_ledgeGrabbedPos.position, _playerStandPos.position);
            }
        }
    }
}