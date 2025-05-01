using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _moveForce;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private ForceMode _forceMode = ForceMode.Force;
    [SerializeField] private Rigidbody _rigidBody;

    private Vector3 _moveDirection;
    private float _horizontalInput;
    private float _verticalInput;

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        CalculateMovementDirection();
        ApplyMovement();
        LimitSpeed();
    }

    private void GetInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
    }

    private void CalculateMovementDirection()
    {
        Vector3 cameraForward = _camera.forward;
        Vector3 cameraRight = _camera.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        _moveDirection = (cameraForward * _verticalInput + cameraRight * _horizontalInput).normalized;
    }

    private void ApplyMovement()
    {
        if (_moveDirection != Vector3.zero)
        {
            _rigidBody.AddForce(_moveDirection * _moveForce, _forceMode);
        }
    }

    private void LimitSpeed()
    {
        if (_rigidBody.velocity.magnitude > _maxSpeed)
        {
            _rigidBody.velocity = _rigidBody.velocity.normalized * _maxSpeed;
        }
    }
}