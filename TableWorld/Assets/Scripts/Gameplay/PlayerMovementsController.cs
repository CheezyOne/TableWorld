using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _moveForce;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private ForceMode _forceMode = ForceMode.Force;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckDistance = 0.2f;

    private bool _isAbleToMove = true;
    private bool _isSlowedDown;
    private bool _isGrounded;
    private float _slownessMultiplier;
    private Vector3 _moveDirection;
    private float _horizontalInput;
    private float _verticalInput;

    public bool IsGrounded => _isGrounded;

    private const float NORMAL_SPEED_MULTIPLIER = 1f;

    private void Update()
    {
        if (!_isAbleToMove)
            return;

        GetInput();
        CheckGround();
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }
    }

    private void CheckGround()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, _groundLayer);
    }

    private void TryJump()
    {
        if (_isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groundCheckDistance);
    }
#endif

    private void OnGameEnd()
    {
        _rigidbody.isKinematic = true;
        _isAbleToMove = false;
    }

    private void OnEnable()
    {
        EventBus.OnGameEnd += OnGameEnd;
    }

    private void OnDisable()
    {
        EventBus.OnGameEnd -= OnGameEnd;
    }
}