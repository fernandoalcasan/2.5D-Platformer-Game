using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTrigger : MonoBehaviour
{
    [SerializeField]
    private bool _isTop;
    private Player _playerRef;

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

    private void TriggerLadder()
    {
        if (_isTop)
            _playerRef.ClimbLadder();
        else
            _playerRef.LeaveLadder();
    }
}
