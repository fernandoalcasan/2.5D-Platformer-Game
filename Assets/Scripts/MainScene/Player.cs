using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Physics Properties")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _gravity = 1f;
    [SerializeField]
    private float _jumpHeight;
    private float _gravImpulse;
    private bool _canDJ;

    private CharacterController _charController;
    private int _coins;

    public int Coins { get => _coins; }

    void Start()
    {
        _charController = GetComponent<CharacterController>();

        if (_charController is null)
            Debug.LogError("Character controller is NULL");

        Collectable.OnCoinCollected += CoinCollected;
    }

    void Update()
    {
        float horInput = Input.GetAxis("Horizontal");
        Vector3 dir = new Vector3(horInput, 0, 0);
        Vector3 velocity = dir * _speed;

        if (_charController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _gravImpulse = _jumpHeight;
                _canDJ = true;
            }
        }
        else
        {
            _gravImpulse -= _gravity;

            if (Input.GetKeyDown(KeyCode.Space) && _canDJ)
            {
                _gravImpulse += _jumpHeight;
                _canDJ = false;
            }
        }

        velocity.y = _gravImpulse;

        _charController.Move(velocity * Time.deltaTime);
    }

    private void CoinCollected()
    {
        _coins++;
    }

    private void OnDestroy()
    {
        Collectable.OnCoinCollected -= CoinCollected;
    }
}
