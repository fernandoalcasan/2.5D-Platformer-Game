using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class attached to dynamic platforms
public class MovablePlatform : MonoBehaviour
{
    [Header("Platform path")]
    [SerializeField]
    private List<Transform> _waypoints;

    [Header("Platform properties")]
    [SerializeField]
    private float _timeToWaypoint;
    [SerializeField]
    private float _timeToWaitInWaypoint;
    [SerializeField]
    private AudioSource _engineAudio;

    //Help vars
    private int _target;
    private float _timePassed;
    private Vector3 _currentPos;
    private Vector3 _destinyPos;
    private bool _arrived;
    private WaitForSeconds _waypointWait;
    
    //vars and destiny initialization
    private void Start()
    {
        if (_waypoints.Count < 2)
            Debug.LogError("Please select 2 or more waypoints for the platform to move");

        _target = 1;
        _waypointWait = new WaitForSeconds(_timeToWaitInWaypoint);
        SetNewDestination();
    }

    //Method to handle platform movement (essential to parent player)
    private void FixedUpdate()
    {
        if(_timePassed < _timeToWaypoint)
        {
            _timePassed += Time.deltaTime;
            transform.position = Vector3.Lerp(_currentPos, _destinyPos, _timePassed / _timeToWaypoint);

            if (_timePassed >= _timeToWaypoint)
                _arrived = true;
        }

        if(_arrived)
        {
            _arrived = false;
            _engineAudio.Stop();
            StartCoroutine(MoveToNextWaypoint());
        }
    }

    //Method to set a new waypoint as destiny for the platform
    private void SetNewDestination()
    {
        _currentPos = _waypoints[_target - 1].position;
        _destinyPos = _waypoints[_target].position;
        _timePassed = 0f;
        _engineAudio.Play();
    }

    //Coroutine to wait before moving on to the next destination and reverse path when last waypoint is reached
    private IEnumerator MoveToNextWaypoint()
    {
        yield return _waypointWait;
        
        _target++;

        if (_target == _waypoints.Count)
        {
            _waypoints.Reverse();
            _target = 1;
        }

        SetNewDestination();
    }

    //method to parent the player when it's touching the platform
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = this.transform;
        }
    }
    
    //method to unparent the player when it leaves the platform
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
