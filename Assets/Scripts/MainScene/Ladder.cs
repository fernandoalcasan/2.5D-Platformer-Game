using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField]
    private Transform _lowPos, _highPos;

    private bool _inRange;
    private Player _playerRef;

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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = false;
        }
    }

    private void Update()
    {
        if(_inRange && Input.GetKeyDown(KeyCode.C))
        {
            _playerRef.GrabLadder(_lowPos.position, _highPos.position);
        }
    }
}
