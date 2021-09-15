using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private ElevatorPanel _oriPanel;
    [SerializeField]
    private ElevatorPanel _endPanel;

    [SerializeField]
    private Transform _origin, _goal;

    [SerializeField]
    private bool _onOrigin;
    private bool _moving;
    private float _timePassed;
    [SerializeField]
    private float _timeToNextFloor;
    [SerializeField]
    private AudioSource _engineAudio;


    private void Start()
    {
        _timePassed = _timeToNextFloor;
    }


    public void CallElevator(bool isOriginPanelCall)
    {
        if (!_moving)
        {
            if ((isOriginPanelCall && !_onOrigin) || (!isOriginPanelCall && _onOrigin))
                MoveElevator();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_moving)
                MoveElevator();
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.parent = null;
    }

    private void FixedUpdate()
    {
        if (_timePassed < _timeToNextFloor)
        {
            _timePassed += Time.deltaTime;

            if(_onOrigin)
                transform.position = Vector3.Lerp(_origin.position, _goal.position, _timePassed / _timeToNextFloor);
            else
                transform.position = Vector3.Lerp(_goal.position, _origin.position, _timePassed / _timeToNextFloor);

            if (_timePassed >= _timeToNextFloor)
            {
                _onOrigin = !_onOrigin;
                _moving = false;
                _oriPanel.ExchangeLights();
                _endPanel.ExchangeLights();
            }
        }
    }

    private void MoveElevator()
    {
        _moving = true;
        _engineAudio.Play();
        _timePassed = 0f;
    }
}
