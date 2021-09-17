using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class attached to the ladders
public class Ladder : MonoBehaviour
{
    //Positions where player will enter and leave the ladder
    [Header("Player positions")]
    [SerializeField]
    private Transform _lowPos, _highPos;

    //Help vars
    private bool _inRange;
    private Player _playerRef;

    //Method to indicate that player can grab the ladder and get a ref to its component
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = true;
            if(_playerRef is null)
            {
                if (other.TryGetComponent(out Player player))
                    _playerRef = player;
                else
                    Debug.LogError("Player reference is NULL");
            }
        }
    }

    //Method to indicate that player can't grab the ladder
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _inRange = false;
    }

    //Method to check for player's input to grab the ladder
    private void Update()
    {
        if(_inRange && Input.GetKeyDown(KeyCode.C))
        {
            _playerRef.GrabLadder(_lowPos.position, _highPos.position);
            _inRange = false;
        }
    }
}
