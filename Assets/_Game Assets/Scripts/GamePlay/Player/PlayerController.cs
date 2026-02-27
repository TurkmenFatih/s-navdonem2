using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Transform  _oriantationTransform ;
    
    [Header("Movement Settings")]    

    [SerializeField]private KeyCode _movementKey;

    [SerializeField]private float _movementSpeed;

    [Header("Jump Settings")]

    [SerializeField] private KeyCode _jumpKey;

    [SerializeField]private float _jumpForce;

    [SerializeField] private bool _canJump;

    [SerializeField] private float _jumpCooldown;

    [Header("sliding Settings")]

    [SerializeField] private KeyCode _slideKey;
     
    [SerializeField] private float _slideMultipiler; 

    [SerializeField] private float _slideDrag;

    [Header("Ground Chechk Settings")]

    [SerializeField] private float _playerHeight;

    [SerializeField] private LayerMask _groundedLayer;

    [SerializeField] private float _groundDrag;


    private Rigidbody _playerRigiBody ;

    private float _verticalInput , _horizontalInput;

    private Vector3  _movementDirection;

    private bool _isSliding;
   
    void Awake()
    {
        _playerRigiBody = GetComponent<Rigidbody>();
        _playerRigiBody.freezeRotation=true;
    }

    void Update()
    {
        SetInputs();
        SetPlayerDrag();
        PlayerSpeedLimit();
        
    }

    void FixedUpdate()
    {
        SetPlayerMovement();
    }

    private void SetPlayerDrag()
    {
        if (_isSliding)
        {
            _playerRigiBody.linearDamping=_slideDrag;
        }
        else
        {
             _playerRigiBody.linearDamping=_groundDrag;
        }
    }

    private void PlayerSpeedLimit()
    {
        Vector3 flatVecolity = new  Vector3(_playerRigiBody.linearVelocity.x , 0f , _playerRigiBody.linearVelocity.z);

        if (flatVecolity.magnitude> _movementSpeed)
        {
            Vector3 limitedVelocity =  flatVecolity.normalized*_movementSpeed;
            _playerRigiBody.linearVelocity= new Vector3(limitedVelocity.x, _playerRigiBody.linearVelocity.y,limitedVelocity.z);
        }
    }

    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical") ;
        if (Input.GetKeyDown(_slideKey))
        {
            _isSliding=true;
            Debug.Log("Player Sliding");
        }
        else if (Input.GetKeyDown(_movementKey))
        {
            _isSliding=false;
            Debug.Log("Player Moving");
        }

        else if (Input.GetKey(_jumpKey) && _canJump &&IsGrounded())
        {
            //ZIPLAMA İŞLEMİ YAPACAK! //
            _canJump=false;
            SetPlayerJumping();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    private void SetPlayerMovement()
    {
        _movementDirection = _oriantationTransform.forward * _verticalInput 
        + _oriantationTransform.right * _horizontalInput;
       if (_isSliding)
       {
         _playerRigiBody.AddForce(_movementDirection.normalized * _movementSpeed* _slideMultipiler ,ForceMode.Force );
       }

       else
       {
         _playerRigiBody.AddForce(_movementDirection.normalized * _movementSpeed ,ForceMode.Force );
       }
        
    }
    private void SetPlayerJumping()
    {
        _playerRigiBody.linearVelocity = new Vector3(_playerRigiBody.linearVelocity.x , 0f , _playerRigiBody.linearVelocity.z);
        _playerRigiBody.AddForce(transform.up * _jumpForce ,ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _canJump=true;
    }
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight* 0.5f+ 0.2f ,_groundedLayer);
    }
}