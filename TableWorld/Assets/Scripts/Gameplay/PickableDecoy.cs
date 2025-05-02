using UnityEngine;
using DG.Tweening;

public class PickableDecoy : MonoBehaviour
{
    [SerializeField] private float _floatingDistance;
    [SerializeField] private float _floatingTime;
    [SerializeField] private GameObject _pickedUpEffect;
    [SerializeField] private float _effectLifetime;
    [SerializeField] private Ease _ease;
    [SerializeField] private float _rotationSpeed;

    private void Awake()
    {
        transform.DOMoveY(transform.position.y + _floatingDistance, _floatingTime).SetEase(_ease).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerIntentory playerInventory))
        {
            playerInventory.AddDecoy();
            Destroy(Instantiate(_pickedUpEffect, transform.position, Quaternion.identity), _effectLifetime);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}