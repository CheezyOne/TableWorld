using UnityEngine;
using System.Collections.Generic;

public class Decoy : MonoBehaviour
{
    [SerializeField] private float _decoyRadius;
    [SerializeField] private Rigidbody _rigidbody;

    private bool _isActivated;
    private List<GameObject> _foundMice = new();

    public Rigidbody Rigidbody => _rigidbody;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isActivated)
            return;

        _isActivated=true;
        BecomeActive();
    }

    private void BecomeActive()
    {
        bool foundMouse = false;
        _rigidbody.isKinematic = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, _decoyRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out NormalMouse normalMouse))
            {
                if (_foundMice.Contains(normalMouse.gameObject))
                    continue;

                _foundMice.Add(normalMouse.gameObject);
                foundMouse = true;
                normalMouse.SetTemporalTarget(transform);
            }
        }

        if (!foundMouse)
            Destroy(gameObject);
    }
}