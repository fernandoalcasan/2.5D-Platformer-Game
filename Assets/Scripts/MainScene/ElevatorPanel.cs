using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanel : MonoBehaviour
{
    [SerializeField]
    private int _coinsRequired;

    private Player _playerRef;
    private bool _inRange;

    [SerializeField]
    private Elevator _elevator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_playerRef is null)
                _playerRef = other.GetComponent<Player>();
            _inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _inRange = false;
    }

    private void Update()
    {
        if(_inRange && Input.GetKeyDown(KeyCode.E))
        {
            if (_playerRef.Coins >= _coinsRequired)
                _elevator.CallElevator();
            else
                Debug.Log("You need " + _coinsRequired + " coins to call the elevator");
        }
    }
}
