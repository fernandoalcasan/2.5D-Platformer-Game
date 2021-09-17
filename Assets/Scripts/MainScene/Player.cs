using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class attached to the player
public class Player : MonoBehaviour
{
    //Action delegate to indicate that player dies or wins the game
    public static Action<bool> OnMissionEnd;

    //Player stats
    private static int _power;
    public static int Power { get => _power; }
    private int _powerRequired = 20;
    private static int _lives;
    public static int Lives { get => _lives; }
    
    //Player movement
    [Header("Physics properties")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpPower;
    [SerializeField]
    private float _gravity;
    [SerializeField]
    private float _climbSpeed;
    
    //Physics help vars
    private float _direction;
    private Vector3 _movement;
    private Vector3 _facing;
    private float _rollingSpeed;
    private float _climbDir;

    //References
    private CharacterController _player;
    private SkinnedMeshRenderer _renderer;
    private Animator _anim;

    //Help vars
    private bool _onLadder;
    private bool _running;
    private bool _jumping;
    private bool _ledgeGrabbed;
    private Vector3 _currentStandLadderPos;
    private Transform _currentStandLedgePos;
    private bool _currentLedgeIsLeft;
    private bool _rolling;
    private bool _landing;
    private Vector3 _initialColliderPos;
    private Vector3 _rollingColliderFix;

    //Flicker behavior vars (player respawn)
    [Header("Respawn properties")]
    [SerializeField]
    private Transform _respawnPos;
    [SerializeField]
    private int _timesToFlick;
    [SerializeField]
    private float _timeBetFlick;
    private WaitForSeconds _wait;
    
    //Audio
    [SerializeField]
    private AudioClip[] _audioClips;

    //Vars initialization and Action delegates subscription
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

    //Method to enable character controller (used by delegate)
    private void EnableController()
    {
        _player.enabled = true;
    }

    //Method called every frame to handle the player depending on its state
    void Update()
    {
        CalculateMovement();

        if (_ledgeGrabbed)
            HandleLedgeInput();
        else if (_onLadder)
            HandleLadderInput();
    }
    
    //Method to handle the actions from the player if character controller is enabled
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

    //Method to calculate and handle the horizontal movement
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

    //Method to handle movement when player runs
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

    //Method to handle movement when player rolls
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

    //Method to handle gravity using the character controller isGrounded property
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

    //Method to handle movement when the player jumps
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

    //Method to handle input when the player is grabbing a ledge
    private void HandleLedgeInput()
    {
        if (_currentLedgeIsLeft)
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

    //Method to handle input when the player is using a ladder
    private void HandleLadderInput()
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

    //Method to reset the boolean values after some animations are executed
    private void ResetBools()
    {
        _running = false;
        _jumping = false;
        _rolling = false;
    }

    //Method to indicate the player how to grab the respective ledge
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

    //Method to change player's position when a ledge has been climbed (due to animation bake)
    public void StandFromLedge()
    {
        transform.position = _currentStandLedgePos.position;
        _anim.SetBool("LedgeGrabbed", false);
        _ledgeGrabbed = false;
        _player.enabled = true;
        transform.parent = null;
    }

    //Method to indicate the player that rolling animation has ended
    public void StopRolling()
    {
        _anim.SetBool("Roll", false);
        _rolling = false;
        _player.center = _initialColliderPos;
    }

    //Method to indicate that a collectable has been collected (used by action delegate)
    private void CollectCollectable()
    {
        _power++;
        if (_power == _powerRequired && !(OnMissionEnd is null))
            OnMissionEnd(true);
    }

    //Method to indicate the player how to grab the respective ladder
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

    //Method to trigger the climbing animation for the ladder
    public void ClimbLadder()
    {
        if(_onLadder)
        {
            _onLadder = false;
            _anim.SetTrigger("ClimbLadder");
        }
    }

    //Method to change player's position when climbing animation for the ladder has ended
    public void StandFromLadder()
    {
        transform.position = _currentStandLadderPos;
        _player.enabled = true;
    }

    //Method to leave the ladder and return to previous state
    public void LeaveLadder()
    {
        _onLadder = false;
        _anim.SetTrigger("LeaveLadder");
        _player.enabled = true;
    }

    //Method to receive damage from a fall (used by action delegate)
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

    //Coroutine to enable player's controller after dying
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

    //Method to play footsteps SFX (through animation events)
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

    //Unsubscribe from action delegates as best practice
    private void OnDisable()
    {
        Collectable.OnCollection -= CollectCollectable;
        DeadZone.OnPlayerFall -= FallDamage;
        UIManager.OnMissionDisplayed -= EnableController;
    }
}
