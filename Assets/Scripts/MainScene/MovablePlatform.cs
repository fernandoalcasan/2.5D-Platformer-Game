using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
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
        if (transform.position == _waypoints[_target].position)
        {
            _target++;

            if (_target == _waypoints.Count)
            {
                _waypoints.Reverse();
                _target = 1;
            }
        }
        else
        {
            float step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _waypoints[_target].position, step);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
