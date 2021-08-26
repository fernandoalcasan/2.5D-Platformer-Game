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
    private float _speed;

    [SerializeField]
    private Transform _origin, _goal;

    [SerializeField]
    private bool _onOrigin;
    private bool _moving;

    public void CallElevator(bool isOriginPanelCall)
    {
        if (!_moving)
        {
            if(isOriginPanelCall && !_onOrigin)
                StartCoroutine(MoveElevator(_origin.position, true));
            else if(!isOriginPanelCall && _onOrigin)
                StartCoroutine(MoveElevator(_goal.position, false));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!_moving)
            {
                if (_onOrigin)
                    StartCoroutine(MoveElevator(_goal.position, false));
                else
                    StartCoroutine(MoveElevator(_origin.position, true));
            }
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.parent = null;
    }

    private IEnumerator MoveElevator(Vector3 pos, bool goingOrigin)
    {
        _moving = true;

        float timePassed = 0f;
        Vector3 currentPos = _onOrigin ? _origin.position : _goal.position;
        
        while (timePassed < 5f)
        {
            timePassed += Time.deltaTime;
            transform.position = Vector3.Lerp(currentPos, pos, timePassed / 5f);
            yield return null;
        }

        _oriPanel.ExchangeLights();
        _endPanel.ExchangeLights();
        _moving = false;
        _onOrigin = goingOrigin;
    }
}
