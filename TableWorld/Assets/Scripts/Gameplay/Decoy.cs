using System;
using System.Collections;
using UnityEngine;

public class Decoy : MonoBehaviour
{
    [SerializeField] private float _decoyRadius;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private int _maxActivations;

    private WaitForSeconds _activationIntervalWait = new(0.5f);
    private int _activationCount;
    private bool _isActivated;

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
        _rigidbody.isKinematic = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, _decoyRadius);

        Array.Sort(colliders, (a, b) =>
            Vector3.Distance(transform.position, a.transform.position)
            .CompareTo(
                Vector3.Distance(transform.position, b.transform.position)
            )
        );

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out NormalMouse normalMouse))
            {
                if (normalMouse.IsEating)
                    continue;

                StopAllCoroutines();
                normalMouse.SetDecoyTarget(transform);
                return;
            }
        }


        if (_activationCount > _maxActivations)
        {
            Destroy(gameObject);
        }

        _activationCount++;
        StartCoroutine(BeActiveRoutine());
    }

    private IEnumerator BeActiveRoutine()
    {
        yield return _activationIntervalWait;
        BecomeActive();
    }
}