using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanel : MonoBehaviour
{

    [SerializeField]
    private Elevator _elevator;

    [SerializeField]
    private GameObject _greenLight;
    [SerializeField]
    private GameObject _redLight;
    
    private bool _inRange;
    [SerializeField]
    private bool _isOriginPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _inRange = false;
    }

    private void Update()
    {
        if (_inRange && Input.GetKeyDown(KeyCode.Q))
            _elevator.CallElevator(_isOriginPanel);
    }

    public void ExchangeLights()
    {
        _redLight.SetActive(!_redLight.activeInHierarchy);
        _greenLight.SetActive(!_greenLight.activeInHierarchy);
    }
}
