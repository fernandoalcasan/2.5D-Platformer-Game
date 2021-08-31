using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _waypoints;
    private int _target;

    [SerializeField]
    private float _timeToWaypoint;
    private float _timePassed;
    private Vector3 _currentPos;
    private Vector3 _destinyPos;

    private void Start()
    {
        if (_waypoints.Count < 2)
            Debug.LogError("Please select 2 or more waypoints for the platform to move");

        _target = 1;
        StartCoroutine(MoveToNextWaypoint());
    }

    private IEnumerator MoveToNextWaypoint()
    {
        _timePassed = 0f;
        _currentPos = _waypoints[_target - 1].position;
        _destinyPos = _waypoints[_target].position;

        while (_timePassed < _timeToWaypoint)
        {
            _timePassed += Time.deltaTime;
            transform.position = Vector3.Lerp(_currentPos, _destinyPos, _timePassed / _timeToWaypoint);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        _target++;

        if (_target == _waypoints.Count)
        {
            _waypoints.Reverse();
            _target = 1;
        }

        StartCoroutine(MoveToNextWaypoint());
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
