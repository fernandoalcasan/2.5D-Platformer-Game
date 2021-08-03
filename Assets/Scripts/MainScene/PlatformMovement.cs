using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _waypoints;
    private int _target;

    [SerializeField]
    private float _speed;

    private void Start()
    {
        if (_waypoints.Count < 2)
            Debug.LogError("Please select 2 or more waypoints for the platform to move");
    }

    private void FixedUpdate()
    {
        float step = _speed * Time.deltaTime;

        if(transform.position == _waypoints[_target].position)
        {
            _target++;
            
            if (_target == _waypoints.Count)
            {
                _waypoints.Reverse();
                _target = 1;
            }

            transform.position = Vector3.MoveTowards(transform.position, _waypoints[_target].position, step);
        }
        else if(transform.position != _waypoints[_target].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _waypoints[_target].position, step);
        }
    }
}
