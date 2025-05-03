using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private float _expansionSpeed = 5f;
    [SerializeField] private float _maxRadius = 15f;
    [SerializeField] private float _waveWidth = 0.5f;
    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private Color _waveColor = Color.red;

    [Header("Physics Settings")]
    [SerializeField] private float _maxForce = 10f;
    [SerializeField] private float _forceFalloff = 1.5f;
    [SerializeField] private ForceMode _forceMode = ForceMode.Impulse;
    [SerializeField] private float _playerDamage;

    private LineRenderer _lineRenderer;
    private HashSet<Collider> _hitObjects = new HashSet<Collider>();
    private float _currentRadius;
    private Vector3 _origin;
    private float _fadeStartRadius;
    private Material _waveMaterial;

    private const float FADE_PERCENTAGE = 0.8f;

    private void Awake()
    {
        InitializeRenderer();
        _origin = transform.position;
        _fadeStartRadius = _maxRadius * FADE_PERCENTAGE;
        ClearPositions();
        StartWave();
    }

    private void ClearPositions()
    {
        Vector3[] positions = new Vector3[_lineRenderer.positionCount];

        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            positions[i] = Vector3.zero;
        }

        _lineRenderer.SetPositions(positions);
    }

    private void InitializeRenderer()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Используем шейдер, поддерживающий прозрачность
        _waveMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended"));
        _waveMaterial.color = _waveColor;

        _lineRenderer.material = _waveMaterial;
        _lineRenderer.startWidth = _waveWidth;
        _lineRenderer.endWidth = _waveWidth;
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = 128;

        // Включаем поддержку прозрачности
        _lineRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _lineRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _lineRenderer.material.EnableKeyword("_ALPHABLEND_ON");
    }

    private void StartWave()
    {
        StartCoroutine(ExpandWaveRoutine());
    }

    private IEnumerator ExpandWaveRoutine()
    {
        _currentRadius = 0f;

        while (_currentRadius < _maxRadius)
        {
            UpdateWaveVisual();
            DetectAndPushObjects();
            UpdateFadeEffect();

            _currentRadius += _expansionSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void UpdateWaveVisual()
    {
        Vector3[] positions = new Vector3[_lineRenderer.positionCount];
        float angleStep = 360f / _lineRenderer.positionCount;

        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            positions[i] = _origin + new Vector3(
                Mathf.Cos(angle) * _currentRadius,
                0,
                Mathf.Sin(angle) * _currentRadius
            );
        }

        _lineRenderer.SetPositions(positions);
    }

    private void DetectAndPushObjects()
    {
        Collider[] hits = Physics.OverlapSphere(_origin, _currentRadius, _detectionLayer);

        foreach (Collider hit in hits)
        {
            if (!_hitObjects.Contains(hit))
            {
                Vector3 hitPos = hit.transform.position;
                float distance = Vector3.Distance(
                    new Vector3(hitPos.x, _origin.y, hitPos.z),
                    _origin
                );

                if (Mathf.Abs(distance - _currentRadius) < _waveWidth * 2)
                {
                    if (hit.TryGetComponent(out PlayerHealth playerHealth))
                    {
                        if (hit.GetComponent<PlayerMovementController>().IsGrounded)
                        {
                            playerHealth.TakeDamage(_playerDamage);
                            ApplyForceToObject(hit, distance);
                        }

                        _hitObjects.Add(hit);
                    }
                }
            }
        }
    }

    private void ApplyForceToObject(Collider hit, float hitDistance)
    {
        Rigidbody rb = hit.attachedRigidbody;

        if (rb != null)
        {
            Vector3 direction = (hit.transform.position - _origin).normalized;
            direction.y = 0; // Оставляем только горизонтальное отталкивание

            // Рассчитываем силу с учетом расстояния (обратная пропорция)
            float forceMultiplier = Mathf.Pow(1 - (hitDistance / _maxRadius), _forceFalloff);
            float appliedForce = _maxForce * forceMultiplier;

            rb.AddForce(direction * appliedForce, _forceMode);
        }
    }

    private void UpdateFadeEffect()
    {
        if (_currentRadius >= _fadeStartRadius)
        {
            float fadeProgress = (_currentRadius - _fadeStartRadius) / (_maxRadius - _fadeStartRadius);
            float alpha = Mathf.Lerp(1f, 0f, fadeProgress);

            // Обновляем только альфа-канал, сохраняя цвет
            Color fadedColor = new Color(
                _waveColor.r,
                _waveColor.g,
                _waveColor.b,
                alpha
            );

            // Применяем ко всем свойствам материала
            _waveMaterial.color = fadedColor;
            _lineRenderer.startColor = fadedColor;
            _lineRenderer.endColor = fadedColor;

            // Явно обновляем материал
            _lineRenderer.sharedMaterial = _waveMaterial;
        }
    }
}