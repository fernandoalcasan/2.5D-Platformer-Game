using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _light;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private Transform _origin, _goal;

    private bool _onOrigin, _moving;

    public void CallElevator()
    {
        if(!_moving && !_onOrigin)
        {
            _light.material.color = Color.green;
            _moving = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!_moving)
            {
                _moving = true;
            }
            
            if(_onOrigin)
                _light.material.color = Color.red;
            else
                _light.material.color = Color.green;

            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.parent = null;
    }

    private void MoveTo(Vector3 pos)
    {
        float step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pos, step);
    }

    private void FixedUpdate()
    {
        if(_moving)
        {
            
            if (_onOrigin)
            {
                if (transform.position == _goal.position)
                {
                    _moving = false;
                    _onOrigin = false;
                }
                else
                    MoveTo(_goal.position);
            }
            else
            {
                if (transform.position == _origin.position)
                {
                    _moving = false;
                    _onOrigin = true;
                }
                else
                    MoveTo(_origin.position);
            }
        }
    }
}
