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
    [SerializeField] private Material _waveMaterial;
    [SerializeField] private float _emissionIntensity = 2f;

    [Header("Physics Settings")]
    [SerializeField] private float _maxForce = 10f;
    [SerializeField] private float _forceFalloff = 1.5f;
    [SerializeField] private ForceMode _forceMode = ForceMode.Impulse;
    [SerializeField] private float _playerDamage;
    [SerializeField] private LineRenderer _lineRenderer;

    private HashSet<Collider> _hitObjects = new HashSet<Collider>();
    private float _currentRadius;
    private Vector3 _origin;
    private float _fadeStartRadius;
    private Material _fadeMaterial;

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
        _fadeMaterial = new Material(_waveMaterial);
        _lineRenderer.material = _fadeMaterial;
        _lineRenderer.startWidth = _waveWidth;
        _lineRenderer.endWidth = _waveWidth;
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = 128;
        _lineRenderer.enabled = true;

        _fadeMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _fadeMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _fadeMaterial.EnableKeyword("_ALPHABLEND_ON");
        _fadeMaterial.EnableKeyword("_EMISSION");

        Color baseColor = _fadeMaterial.color;
        Color emissionColor = new Color(
            Mathf.Clamp(baseColor.r * _emissionIntensity, 0, 1),
            Mathf.Clamp(baseColor.g * _emissionIntensity, 0, 1),
            Mathf.Clamp(baseColor.b * _emissionIntensity, 0, 1)
        );
        _fadeMaterial.SetColor("_EmissionColor", emissionColor);
        _fadeMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
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
            direction.y = 0; 
            float forceMultiplier = Mathf.Pow(1 - (hitDistance / _maxRadius), _forceFalloff);
            float appliedForce = _maxForce * forceMultiplier;
            rb.AddForce(direction * appliedForce, _forceMode);
        }
    }

    private void UpdateFadeEffect()
    {
        float alpha = 1f;

        if (_currentRadius >= _fadeStartRadius)
        {
            float fadeProgress = (_currentRadius - _fadeStartRadius) / (_maxRadius - _fadeStartRadius);
            alpha = Mathf.Lerp(1f, 0f, fadeProgress);
        }

        Color fadedColor = new Color(
            _fadeMaterial.color.r,
            _fadeMaterial.color.g,
            _fadeMaterial.color.b,
            alpha
        );

        Color emissionColor = new Color(
            Mathf.Clamp(_fadeMaterial.color.r * _emissionIntensity, 0, 1),
            Mathf.Clamp(_fadeMaterial.color.g * _emissionIntensity, 0, 1),
            Mathf.Clamp(_fadeMaterial.color.b * _emissionIntensity, 0, 1)
        ) * alpha;

        _fadeMaterial.color = fadedColor;
        _fadeMaterial.SetColor("_EmissionColor", emissionColor);
        _lineRenderer.startColor = fadedColor;
        _lineRenderer.endColor = fadedColor;

        if (_fadeMaterial.HasProperty("_EmissionIntensity"))
        {
            _fadeMaterial.SetFloat("_EmissionIntensity", _emissionIntensity * alpha);
        }
    }
}