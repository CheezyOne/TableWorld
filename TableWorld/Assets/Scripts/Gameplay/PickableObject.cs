using DG.Tweening;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField] private float _floatingDistance;
    [SerializeField] private float _floatingTime;
    [SerializeField] private Ease _ease;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private GameObject _pickedUpEffect;
    [SerializeField] private float _effectLifetime;

    private void Awake()
    {
        transform.DOMoveY(transform.position.y + _floatingDistance, _floatingTime).SetEase(_ease).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }

    protected void PickedUpEffect()
    {
        Destroy(Instantiate(_pickedUpEffect, transform.position, Quaternion.identity), _effectLifetime);
        Destroy(gameObject);
    }
}