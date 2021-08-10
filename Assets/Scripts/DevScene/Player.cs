using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DevScene
{
    public class Player : MonoBehaviour
    {
        [Header("Physics Properties")]
        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _pushPower;

        private Vector3 _direction;
        private Vector3 _velocity;
        [SerializeField]
        private float _gravity = 1f;
        [SerializeField]
        private float _jumpHeight;
        private float _gravImpulse;
        private bool _canDoubleJump;
        private bool _canWallJump;
        private Vector3 _wallNormal;

        private CharacterController _charController;

        private int _coins;
        public int Coins { get => _coins; }

        private int _lives = 3;
        public int Lives { get => _lives; }

        [SerializeField]
        private Transform _respawnPoint;

        void Start()
        {
            _charController = GetComponent<CharacterController>();

            if (_charController is null)
                Debug.LogError("Character controller is NULL");

            Collectable.OnCoinCollected += CoinCollected;
            DeadZone.OnPlayerFall += ReceiveDamage;
        }

        void Update()
        {
            if (!_charController.enabled)
                return;

            float horInput = Input.GetAxis("Horizontal");

            if (_charController.isGrounded)
            {
                _canWallJump = false;
                _direction = new Vector3(horInput, 0, 0);
                _velocity = _direction * _speed;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _gravImpulse = _jumpHeight;
                    _canDoubleJump = true;
                }
            }
            else
            {
                _gravImpulse -= _gravity;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (_canWallJump)
                    {
                        _gravImpulse = _jumpHeight;
                        _velocity = _wallNormal * _speed;
                        _canWallJump = false;
                        _canDoubleJump = false;
                    }
                    else if (_canDoubleJump)
                    {
                        _gravImpulse += _jumpHeight;
                        _canDoubleJump = false;
                    }
                }
            }

            _velocity.y = _gravImpulse;

            _charController.Move(_velocity * Time.deltaTime);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.collider.CompareTag("MovableBox"))
            {
                Rigidbody box = hit.collider.attachedRigidbody;

                if (!(box is null))
                {
                    Vector3 moveDir = new Vector3(hit.moveDirection.x, 0, 0);
                    box.velocity = moveDir * _pushPower;
                }
            }

            if (!_charController.isGrounded && hit.collider.CompareTag("Wall"))
            {
                _wallNormal = hit.normal;
                _canWallJump = true;
            }
        }

        private void CoinCollected()
        {
            _coins++;
        }

        private void ReceiveDamage()
        {
            _lives--;

            if (_lives < 1)
                SceneManager.LoadScene(0);

            _charController.enabled = false;
            transform.position = _respawnPoint.position;
            StartCoroutine(EnableController());
        }

        private IEnumerator EnableController()
        {
            yield return new WaitForSeconds(0.5f);
            _charController.enabled = true;
        }

        private void OnDestroy()
        {
            Collectable.OnCoinCollected -= CoinCollected;
            DeadZone.OnPlayerFall -= ReceiveDamage;
        }
    }
}