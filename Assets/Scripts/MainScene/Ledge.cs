using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class attached to the ledge colliders
public class Ledge : MonoBehaviour
{
    //Positions where player will hang and stand from the ledge
    [Header("Player positions")]
    [SerializeField]
    private Transform _ledgeGrabbedPos, _playerStandPos;

    //To indicate if the ledge grabbed is on the left or right side of the platform
    [Header("Ledge properties")]
    [SerializeField]
    private bool _isLeft;

    //Help vars
    private Player _playerRef;

    //Method to get a reference to the player and handle the grab mechanic
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LedgeGrabChecker"))
        {
            if(_playerRef is null)
            {
                if (other.transform.parent.TryGetComponent(out Player player))
                {
                    _playerRef = player;
                    GrabLedge();
                }
                else
                    Debug.LogError("Player componnet is NULL");
            }
            else
                GrabLedge();
            
            other.transform.parent.parent = gameObject.transform;
        }
    }

    //Method that indicates the player to climb the ledge
    private void GrabLedge()
    {
        _playerRef.GrabLedge(_ledgeGrabbedPos.position, _playerStandPos.transform, _isLeft);
    }
}
