using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class attached to ladder triggers to leave the ladder
public class LadderTrigger : MonoBehaviour
{
    [Header("Trigger properties")]
    [SerializeField]
    private bool _isTop;

    //Help vars
    private Player _playerRef;

    //Method to get reference of the player and leave the ladder respectively
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LadderClimbChecker"))
        {
            if (_playerRef is null)
            {
                if (other.transform.parent.TryGetComponent(out Player player))
                {
                    _playerRef = player;
                    TriggerLadder();
                }
                else
                    Debug.LogError("Player reference is NULL");
            }
            else
                TriggerLadder();
        }
    }

    //Method that indicates the player to leave the ladder
    private void TriggerLadder()
    {
        if (_isTop)
            _playerRef.ClimbLadder();
        else
            _playerRef.LeaveLadder();
    }
}
