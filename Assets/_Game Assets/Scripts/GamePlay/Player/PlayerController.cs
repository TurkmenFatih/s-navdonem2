using System;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action OnPlayerJumped;
    public Action <PlayerState> OnPlayerStateChanged;

    [Header("References")]

    [SerializeField] private Transform _oriantationTransform;

    [Header("Movement Settings")]

    [SerializeField] private KeyCode _movementKey;

    [SerializeField] private float _movementSpeed;

    [Header("Jump Settings")]

    [SerializeField] private KeyCode _jumpKey;

    [SerializeField] private float _jumpForce;

    [SerializeField] private float _jumpCooldown;

    [SerializeField] private float _airMultiplier;
 
    [SerializeField] private float _airDrag;

    [SerializeField] private bool _canJump;

    [Header("sliding Settings")]

    [SerializeField] private KeyCode _slideKey;

    [SerializeField] private float _slideMultipiler;

    [SerializeField] private float _slideDrag;

    [Header("Ground Chechk Settings")]

    [SerializeField] private float _playerHeight;

    [SerializeField] private LayerMask _groundedLayer;

    [SerializeField] private float _groundDrag;

    
    private Rigidbody _playerRigiBody;

    private  float _startingMovementSpeed, _startingJumpForce;

    private float _verticalInput, _horizontalInput;

    private Vector3 _movementDirection;

    private bool _isSliding;
   
    private StateController _stateController;

    void Awake()
    {
        _stateController = GetComponent<StateController>();
        _playerRigiBody = GetComponent<Rigidbody>();
        _playerRigiBody.freezeRotation = true;

        _startingMovementSpeed = _movementSpeed ;
        _startingJumpForce = _jumpForce ;
    }

   
    void Update()
    {
        SetInputs();
        SetStates();
        SetPlayerDrag();
        PlayerSpeedLimit();

    }

     private void PlayerController_OnPlayerJumped()
    {
        
    }


    void FixedUpdate()
    {
        SetPlayerMovement();
    }

    private void SetPlayerDrag()
    {
        _playerRigiBody.linearDamping = _stateController.GetCurrentState() switch
        {
         PlayerState.Move  =>_groundDrag,
         PlayerState.Slide =>_slideDrag,
         PlayerState.Jump => _airDrag,
         _ => _playerRigiBody.linearDamping
        };
    }

    private void PlayerSpeedLimit()
    {
        Vector3 flatVecolity = new Vector3(_playerRigiBody.linearVelocity.x, 0f, _playerRigiBody.linearVelocity.z);

        if (flatVecolity.magnitude > _movementSpeed)
        {
            Vector3 limitedVelocity = flatVecolity.normalized * _movementSpeed;
            _playerRigiBody.linearVelocity = new Vector3(limitedVelocity.x, _playerRigiBody.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(_slideKey))
        {
            _isSliding = true;
        }
        else if (Input.GetKeyDown(_movementKey))
        {
            _isSliding = false;
        }

        else if (Input.GetKey(_jumpKey) && _canJump && IsGrounded())
        {
            //ZIPLAMA İŞLEMİ YAPACAK! //
            _canJump = false;
            SetPlayerJumping();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    private void SetStates()
    {
        var _movementDirection = GetMovemenetDirection();
        var isGrounded = IsGrounded();
        var _isSliding = IsSliding();
        var currentState = _stateController.GetCurrentState();

        var newState = currentState switch
        {
            _ when _movementDirection == Vector3.zero && isGrounded && !_isSliding => PlayerState.Idle,
            _ when _movementDirection != Vector3.zero && isGrounded && !_isSliding => PlayerState.Move,
            _ when _movementDirection != Vector3.zero && isGrounded && _isSliding => PlayerState.Slide,
            _ when _movementDirection == Vector3.zero && isGrounded && _isSliding => PlayerState.SlideIdle,
            _ when !_canJump && !isGrounded => PlayerState.Jump,
            _ => currentState
        };
        if (newState != currentState)
        {
            _stateController.ChangeState(newState);
            OnPlayerStateChanged?.Invoke(newState);
        }

    }

    private void SetPlayerMovement()
    {
        _movementDirection = _oriantationTransform.forward * _verticalInput
        + _oriantationTransform.right * _horizontalInput;

        float forceMultiplier = _stateController.GetCurrentState() switch
        {
            PlayerState.Move => 1f,
            PlayerState.Slide => _slideMultipiler,
            PlayerState.Jump => _airMultiplier,
            _ => 1f
        };

        _playerRigiBody.AddForce(_movementDirection.normalized * _movementSpeed * forceMultiplier, ForceMode.Force);
    }
    private void SetPlayerJumping()
    {
        OnPlayerJumped?.Invoke();
        _playerRigiBody.linearVelocity = new Vector3(_playerRigiBody.linearVelocity.x, 0f, _playerRigiBody.linearVelocity.z);
        _playerRigiBody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _canJump = true;
    }
    #region  Helper Functions
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundedLayer);
    }

    private Vector3 GetMovemenetDirection()
    {
        return _movementDirection.normalized;
    }
    public bool IsSliding()
    {
        return _isSliding;
    }

    public void SetMovementSpeed(float speed, float duration)
    {
        _movementSpeed += speed;
        Invoke(nameof(ResetMovementSpeed),duration);
    }

    public void ResetMovementSpeed()
    {
        _movementSpeed = _startingMovementSpeed;
    }
    
     public void SetJumpForce(float speed, float duration)
    {
        _jumpForce += speed;
        Invoke(nameof(ResetJumpForce),duration);
    }

    public void ResetJumpForce()
    {
        _jumpForce = _startingJumpForce;
    }

    public Rigidbody GetPlayerRigibody()
    {
        return _playerRigiBody;
    }

    #endregion
}