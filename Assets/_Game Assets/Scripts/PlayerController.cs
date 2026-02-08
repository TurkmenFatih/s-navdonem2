using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Transform  _oriantationTransform ;
    
    [Header("Movement Settings")]    

    [SerializeField]private float _movementSpeed;

    [Header("Jump Settings")]

    [SerializeField] private KeyCode _jumpKey;

    [SerializeField]private float _jumpForce;

    [SerializeField] private bool _canJump;

    [SerializeField] private float _jumpCooldown;

    [Header("Ground Chechk Setting")]

    [SerializeField] private float _playerHeight;

    [SerializeField] private LayerMask _groundedLayer;


    private Rigidbody _playerRigiBody ;

    private float _verticalInput , _horizontalInput;

    private Vector3  _movementDirection;

   
    void Awake()
    {
        _playerRigiBody = GetComponent<Rigidbody>();
        _playerRigiBody.freezeRotation=true;
    }

    void Update()
    {
        SetInputs();
        
    }

    void FixedUpdate()
    {
        SetPlayerMovement();
    }

    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical") ;
        if (Input.GetKey(_jumpKey) && _canJump &&IsGrounded())
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

        _playerRigiBody.AddForce(_movementDirection.normalized * _movementSpeed ,ForceMode.Force );
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