using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _moveForce;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private ForceMode _forceMode = ForceMode.Force;
    [SerializeField] private Rigidbody _rigidbody;

    private bool _isSlowedDown;
    private float _slownessMultiplier;
    private Vector3 _moveDirection;
    private float _horizontalInput;
    private float _verticalInput;

    private const float NORMAL_SPEED_MULTIPLIER = 1f;

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
        float currentMoveForce = _moveForce;
        currentMoveForce *= _isSlowedDown ? _slownessMultiplier : NORMAL_SPEED_MULTIPLIER;

        if (_moveDirection != Vector3.zero)
        {
            _rigidbody.AddForce(_moveDirection * currentMoveForce, _forceMode);
        }
    }

    private void LimitSpeed()
    {
        if (_rigidbody.velocity.magnitude > _maxSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
        }
    }

    public void SlowDown(float slownessMultiplier)
    {
        _isSlowedDown = true;
        _slownessMultiplier = slownessMultiplier;
    }

    public void StopSlowingDown()
    {
        _isSlowedDown = false;
    }
}