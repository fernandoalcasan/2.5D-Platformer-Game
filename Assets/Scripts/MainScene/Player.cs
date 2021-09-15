using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static Action<bool> OnMissionEnd;

    //Player stats
    private static int _power;
    public static int Power { get => _power; }
    private int _powerRequired = 20;
    private static int _lives;
    public static int Lives { get => _lives; }
    
    //Player movement
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpPower;
    private float _direction;
    [SerializeField]
    private float _gravity;
    private Vector3 _movement;
    private Vector3 _facing;
    private float _rollingSpeed;
    private float _climbDir;
    [SerializeField]
    private float _climbSpeed;

    //References
    private CharacterController _player;
    private SkinnedMeshRenderer _renderer;
    private Animator _anim;
    [SerializeField]
    private Transform _respawnPos;

    //Help vars
    private bool _onLadder;
    private bool _running;
    private bool _jumping;
    private bool _ledgeGrabbed;
    private Vector3 _currentStandLadderPos;
    private Transform _currentStandLedgePos;
    private bool _currentLedgeIsLeft;
    private bool _rolling;
    [SerializeField]
    private int _timesToFlick;
    [SerializeField]
    private float _timeBetFlick;
    private WaitForSeconds _wait;
    private bool _landing;
    private Vector3 _initialColliderPos;
    private Vector3 _rollingColliderFix;

    //Audio
    [SerializeField]
    private AudioClip[] _audioClips;

    private void OnEnable()
    {
        _power = 0;
        _lives = 3;

        _player = GetComponent<CharacterController>();
        if (_player is null)
            Debug.LogError("Character Controller is NULL");

        _initialColliderPos = _player.center;
        _rollingColliderFix = new Vector3(_player.center.x, _player.center.y, 0.5f);

        _anim = GetComponent<Animator>();
        if (_anim is null)
            Debug.LogError("Animator is NULL");

        _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (_renderer is null)
            Debug.LogError("Skinned mesh renderer is NULL");

        _wait = new WaitForSeconds(_timeBetFlick);

        Collectable.OnCollection += CollectCollectable;
        DeadZone.OnPlayerFall += FallDamage;
        UIManager.OnMissionDisplayed += EnableController;
    }

    private void EnableController()
    {
        _player.enabled = true;
    }

    void Update()
    {
        CalculateMovement();

        if(_ledgeGrabbed)
        {
            if(_currentLedgeIsLeft)
            {
                if (Input.GetAxisRaw("Horizontal") > 0f)
                    _anim.SetTrigger("ClimbUp");
            }
            else
            {
                if (Input.GetAxisRaw("Horizontal") < 0f)
                    _anim.SetTrigger("ClimbUp");
            }
            
        }
        else if(_onLadder)
        {
            _climbDir = Input.GetAxisRaw("Vertical");
            if (_climbDir > 0f)
            {
                transform.Translate(_climbSpeed * Time.deltaTime * Vector3.up);
            }
            else if (_climbDir < 0f)
            {
                transform.Translate(_climbSpeed * Time.deltaTime * Vector3.down);
            }
            _anim.SetFloat("ClimbDir", _climbDir);
        }
    }
    
    private void CalculateMovement()
    {
        if (!_player.enabled)
            return;

        HandleMovement();
        HandleRunning();
        HandleRolling();
        _player.Move(_movement * Time.deltaTime);

        HandleGravity();
        HandleJumping();
    }

    private void HandleMovement()
    {
        _direction = Input.GetAxis("Horizontal");
        _movement.x = _direction * _speed;
        _anim.SetFloat("Speed", Mathf.Abs(_direction));

        if (_direction != 0f)
        {
            _facing = transform.eulerAngles;
            _facing.y = _direction > 0f ? 90f : -90f;
            transform.eulerAngles = _facing;
        }
    }

    private void HandleRunning()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !_rolling && !_jumping)
        {
            _movement.x *= 2f;
            _anim.SetBool("Running", true);
            _running = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _anim.SetBool("Running", false);
            _running = false;
        }
    }

    private void HandleRolling()
    {
        if (Input.GetKeyDown(KeyCode.E) && _direction != 0f
            && _player.isGrounded && !_rolling && !_jumping)
        {
            _anim.SetBool("Roll", true);
            _rolling = true;

            if (_running)
            {
                _player.center = _rollingColliderFix;
                _rollingSpeed = 2f;
            }
            else
                _rollingSpeed = 1.5f;
        }

        if(_rolling)
            _movement.x *= _rollingSpeed;
    }

    private void HandleGravity()
    {
        if(_player.isGrounded)
        {
            if (_landing)
            {
                PlayFootStep(3);
                _landing = false;
            }
            _movement.y = -.5f;
            
            if (_jumping)
            {
                _jumping = false;
                _anim.SetBool("Jumping", false);
            }
        }
        else
        {
            if(_jumping && _movement.y < 0)
                _movement.y -= _gravity * 2.25f * Time.deltaTime;
            else
            {
                _movement.y -= _gravity * Time.deltaTime;
                if(!_landing)
                    _landing = true;
            }
        }
    }

    private void HandleJumping()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _player.isGrounded)
        {
            if (!_rolling && !_jumping)
            {
                if (_running && _movement.x != 0f)
                    _movement.y = _jumpPower * 1.25f;
                else
                    _movement.y = _jumpPower;

                _anim.SetBool("Jumping", true);
                _jumping = true;
            }
        }
    }
    
    private void ResetBools()
    {
        _running = false;
        _jumping = false;
        _rolling = false;
    }

    public void GrabLedge(Vector3 ledgePos, Transform standPosTransform, bool isLeftLedge)
    {
        _player.enabled = false;
        _anim.SetBool("LedgeGrabbed", true);
        AudioManager.Instance.PlayOneShotFullVolume(_audioClips[6]);
        _ledgeGrabbed = true;
        transform.position = ledgePos;
        _currentStandLedgePos = standPosTransform;
        _currentLedgeIsLeft = isLeftLedge;

        _anim.SetFloat("Speed", 0f);
        _anim.SetBool("Running", false);
        _anim.SetBool("Jumping", false);
        ResetBools();
    }

    public void StandFromLedge()
    {
        transform.position = _currentStandLedgePos.position;
        _anim.SetBool("LedgeGrabbed", false);
        _ledgeGrabbed = false;
        _player.enabled = true;
        transform.parent = null;
    }

    public void StopRolling()
    {
        _anim.SetBool("Roll", false);
        _rolling = false;
        _player.center = _initialColliderPos;
    }

    private void CollectCollectable()
    {
        _power++;
        if (_power == _powerRequired && !(OnMissionEnd is null))
            OnMissionEnd(true);
    }

    public void GrabLadder(Vector3 ladderPos, Vector3 standPos)
    {
        transform.position = ladderPos;
        _currentStandLadderPos = standPos;
        _onLadder = true;
        _player.enabled = false;

        _anim.SetTrigger("LadderGrabbed");
        _anim.SetFloat("Speed", 0f);
        _anim.SetBool("Running", false);
        _anim.SetBool("Jumping", false);
        ResetBools();
    }

    public void ClimbLadder()
    {
        //To avoid changing anim trigger when player is just walking through the stair at the top
        if(_onLadder)
        {
            _onLadder = false;
            _anim.SetTrigger("ClimbLadder");
        }
    }

    public void StandFromLadder()
    {
        transform.position = _currentStandLadderPos;
        _player.enabled = true;
    }

    public void LeaveLadder()
    {
        _onLadder = false;
        _anim.SetTrigger("LeaveLadder");
        _player.enabled = true;
    }

    private void FallDamage()
    {
        _lives--;

        if(_lives == 0)
        {
            if (!(OnMissionEnd is null))
                OnMissionEnd(false);
            gameObject.SetActive(false);
        }
        else
        {
            _player.enabled = false;
            transform.position = _respawnPos.position;
            StartCoroutine(EnableControllerAfterDeath());
        }
    }

    private IEnumerator EnableControllerAfterDeath()
    {
        _player.enabled = true;
        for (int i = 0; i < _timesToFlick * 2; i++)
        {
            _renderer.enabled = !_renderer.enabled;
            yield return _wait;
        }
        _renderer.enabled = true;
    }

    private void PlayFootStep(int type)
    {
        if (!_player.isGrounded && !_ledgeGrabbed)
            return;

        switch (type)
        {
            case 0: //Walk
                AudioManager.Instance.PlayOneShot(_audioClips[0], 1f);
                break;
            case 1: //Run
                AudioManager.Instance.PlayOneShot(_audioClips[1], 1f);
                break;
            case 2: //Jump
                AudioManager.Instance.PlayOneShot(_audioClips[2], 1f);
                break;
            case 3: //Land
                AudioManager.Instance.PlayOneShot(_audioClips[3], 1f);
                break;
            case 4: //Roll
                AudioManager.Instance.PlayOneShot(_audioClips[4], 1f);
                break;
            case 5: //Ladder step
                AudioManager.Instance.PlayOneShot(_audioClips[5], 1f);
                break;
            default:
                Debug.LogError("Index out of options");
                break;
        }
    }

    private void OnDisable()
    {
        Collectable.OnCollection -= CollectCollectable;
        DeadZone.OnPlayerFall -= FallDamage;
        UIManager.OnMissionDisplayed -= EnableController;
    }
}
