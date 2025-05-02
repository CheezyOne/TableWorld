using UnityEngine;

public class Decoy : MonoBehaviour
{
    [SerializeField] private float _decoyRadius;
    [SerializeField] private Rigidbody _rigidbody;

    public Rigidbody Rigidbody => _rigidbody;

    private void OnCollisionEnter(Collision collision)
    {
        BecomeActive();
    }

    private void BecomeActive()
    {
        _rigidbody.isKinematic = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, _decoyRadius);

        foreach (Collider collider in colliders)
        {

        }
    }
}