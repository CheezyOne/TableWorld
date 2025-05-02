using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _distance = 5f;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _shakeIntensity = 0.05f;
    [SerializeField] private float _shakeDuration = 0.4f;

    private float _currentRotationY = 0f;
    private bool _isShaking;
    private float _shakeTimer;
    private Vector3 _originalOffset;

    private void Start()
    {
        _originalOffset = _offset;
        UpdateCameraPosition();
        transform.LookAt(_player.position);
    }

    private void Update()
    {
        HandleRotation();

        if (_isShaking)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0f)
            {
                _isShaking = false;
                _offset = _originalOffset;
            }
        }
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void StartShake()
    {
        _isShaking = true;
        _shakeTimer = _shakeDuration;
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * _rotationSpeed;
            _currentRotationY += mouseX;
        }
    }

    private void UpdateCameraPosition()
    {
        if (_player == null) return;

        if (_isShaking)
        {
            Vector3 randomOffset = Random.insideUnitSphere * _shakeIntensity;
            _offset = _originalOffset + randomOffset;
        }

        Quaternion rotation = Quaternion.Euler(0, _currentRotationY, 0);
        Vector3 cameraPosition = _player.position - (rotation * Vector3.forward * _distance);
        transform.position = cameraPosition + _offset;
        transform.LookAt(_player.position + _originalOffset);
    }

    private void OnEnable()
    {
        EventBus.OnStomp += StartShake;
    }

    private void OnDisable()
    {
        EventBus.OnStomp -= StartShake;
    }
}