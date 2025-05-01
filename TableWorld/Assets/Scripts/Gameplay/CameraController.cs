using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _distance = 5f;
    [SerializeField] private Vector3 _offset;

    private float _currentRotationY = 0f;

    private void Start()
    {
        UpdateCameraPosition();
        transform.LookAt(_player.position);
    }

    private void Update()
    {
        HandleRotation();
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
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
        if (_player == null) 
            return;

        Quaternion rotation = Quaternion.Euler(0, _currentRotationY, 0);
        Vector3 cameraPosition = _player.position - (rotation * Vector3.forward * _distance);
        transform.position = cameraPosition + _offset;
        transform.LookAt(_player.position + _offset);
    }
}