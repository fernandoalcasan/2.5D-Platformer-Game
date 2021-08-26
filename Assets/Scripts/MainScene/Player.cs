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
    private Vector3 _facing;
    private CharacterController _player;
    private Animator _anim;
    private bool _running;
    private bool _jumping;
    private bool _ledgeGrabbed;
    private Vector3 _currentStandLedgePos;
    private bool _rolling;
    private float _rollingSpeed;

    void Start()
    {
        _player = GetComponent<CharacterController>();
        if (_player is null)
            Debug.LogError("Character Controller is NULL");
        
        _anim = GetComponentInChildren<Animator>();
        if (_anim is null)
            Debug.LogError("Animator in children is NULL");
    }

    void Update()
    {
        CalculateMovement();

        if(_ledgeGrabbed)
        {
            if(Input.GetAxisRaw("Horizontal") > 0f)
            {
                _anim.SetTrigger("ClimbUp");
            }
        }
    }

    private void CalculateMovement()
    {
        if (!_player.enabled)
            return;

        _direction = Input.GetAxis("Horizontal");
        _movement = new Vector3(_direction * _speed, 0f, 0f);
        _anim.SetFloat("Speed", Mathf.Abs(_direction));

        if (_direction != 0f)
        {
            _facing = transform.eulerAngles;
            _facing.y = _direction > 0f ? 0f : 180f;
            transform.eulerAngles = _facing;
        }

        if (_player.isGrounded)
        {
            if (_jumping)
            {
                _jumping = false;
                _anim.SetBool("Jumping", false);
            }

            if (Input.GetKey(KeyCode.LeftShift) && !_rolling)
            {
                _movement.x *= 2f;
                _anim.SetBool("Running", true);
                _running = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _anim.SetBool("Roll", true);
                _rolling = true;

                if (_running)
                    _rollingSpeed = 2f;
                else
                    _rollingSpeed = 1.5f;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !_rolling)
            {
                if (_running)
                    _gravityImpulse = _jumpPower * 1.25f;
                else
                    _gravityImpulse = _jumpPower;

                _anim.SetBool("Jumping", true);
                _jumping = true;
            }
        }
        else
        {
            _gravityImpulse -= _gravity * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _anim.SetBool("Running", false);
            _running = false;
        }

        if(_rolling)
            _movement.x *= _rollingSpeed;

        _movement.y = _gravityImpulse;
        _player.Move(_movement * Time.deltaTime);
    }

    public void GrabLedge(Vector3 ledgePos, Vector3 standPos)
    {
        _player.enabled = false;
        _anim.SetBool("LedgeGrabbed", true);
        _ledgeGrabbed = true;
        transform.position = ledgePos;
        _currentStandLedgePos = standPos;

        _anim.SetFloat("Speed", 0f);
        _anim.SetBool("Running", false);
        _anim.SetBool("Jumping", false);
    }

    public void StandFromLedge()
    {
        transform.position = _currentStandLedgePos;
        _anim.SetBool("LedgeGrabbed", false);
        _player.enabled = true;
    }

    public void StopRolling()
    {
        _anim.SetBool("Roll", false);
        _rolling = false;
    }
}
