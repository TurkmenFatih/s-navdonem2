using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Transform  _oriantationTransform ;
    
    [Header("Movement Settings")]    

    [SerializeField]private float _movementSpeed;

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
    }

    private void SetPlayerMovement()
    {
        _movementDirection = _oriantationTransform.forward * _verticalInput 
        + _oriantationTransform.right * _horizontalInput;

        _playerRigiBody.AddForce(_movementDirection * _movementSpeed ,ForceMode.Force );
    }
}