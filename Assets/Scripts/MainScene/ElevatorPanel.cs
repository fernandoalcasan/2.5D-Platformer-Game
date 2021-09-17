using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class attached to the elevator panels
public class ElevatorPanel : MonoBehaviour
{
    [Header("Platform ref")]
    [SerializeField]
    private Elevator _elevator;

    [Header("Lights refs")]
    [SerializeField]
    private GameObject _greenLight;
    [SerializeField]
    private GameObject _redLight;
    
    [Header("Panel properties")]
    [SerializeField]
    private bool _isOriginPanel;

    //Help vars
    private bool _inRange;

    //Method to indicate that player is in range to call the elevator platform
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _inRange = true;
    }

    //Method to indicate that player isn't in range to call the elevator platform
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _inRange = false;
    }

    //Method to check for player input to call elevator if player is in range
    private void Update()
    {
        if (_inRange && Input.GetKeyDown(KeyCode.Q))
            _elevator.CallElevator(_isOriginPanel);
    }

    //Method to change the panel light to indicate if platform can be called
    public void ExchangeLights()
    {
        _redLight.SetActive(!_redLight.activeInHierarchy);
        _greenLight.SetActive(!_greenLight.activeInHierarchy);
    }
}
