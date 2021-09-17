using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class attached to the elevator platform
public class Elevator : MonoBehaviour
{
    //Reference to the panels that call the platform
    [Header("Panel Refs")]
    [SerializeField]
    private ElevatorPanel _oriPanel;
    [SerializeField]
    private ElevatorPanel _endPanel;

    [Header("Elevator properties")]
    [SerializeField]
    private Transform _origin, _goal;
    [SerializeField]
    private bool _onOrigin;
    [SerializeField]
    private float _timeToNextFloor;
    [SerializeField]
    private AudioSource _engineAudio;

    //Help vars
    private bool _moving;
    private float _timePassed;

    //Var initialization
    private void Start()
    {
        //To avoid movement at start due to FixedUpdate
        _timePassed = _timeToNextFloor;
    }

    //Method called by the elevator panels to call the platform
    public void CallElevator(bool isOriginPanelCall)
    {
        if (!_moving)
        {
            if ((isOriginPanelCall && !_onOrigin) || (!isOriginPanelCall && _onOrigin))
                MoveElevator();
        }
    }

    //Method to trigger movement and parent the player when it's touching the platform.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_moving)
                MoveElevator();
            other.transform.parent = this.transform;
        }
    }

    //Method to unparent the player when it isn't touching the platform
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.parent = null;
    }

    //Method to move the platform (essential to parent player)
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

    //Method to indicate that platform should move and play SFX
    private void MoveElevator()
    {
        _moving = true;
        _engineAudio.Play();
        _timePassed = 0f;
    }
}
