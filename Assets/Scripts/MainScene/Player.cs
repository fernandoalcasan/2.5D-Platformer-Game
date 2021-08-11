using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpPower;
    private float _direction;
    [SerializeField]
    private float _gravity;
    private float _gravityImpulse;
    private Vector3 _movement;
    private CharacterController _player;

    void Start()
    {
        _player = GetComponent<CharacterController>();
        if (_player is null)
            Debug.LogError("Character Controller is NULL");
    }

    void Update()
    {
        _direction = Input.GetAxis("Horizontal");
        _movement = new Vector3(_direction * _speed, 0f, 0f);

        if(_player.isGrounded)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                _gravityImpulse = _jumpPower;
            }
        }
        else
        {
            _gravityImpulse -= _gravity * Time.deltaTime;            
        }

        _movement.y = _gravityImpulse;
        _player.Move(_movement * Time.deltaTime);
    }
}
