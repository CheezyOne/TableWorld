using UnityEngine;
using DG.Tweening;

public class Mouse : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _pushForce;
    [SerializeField] private ForceMode _pushForceMode;
    [SerializeField] private float _stunTime;
    [SerializeField] private float _stunRotationTime;
    [SerializeField] private Vector3 _stunRotation;

    protected bool _isStunned;

    public bool IsStunned => _isStunned;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isStunned) 
            return;

        if (collision.transform.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(_damage);
            Vector3 direction = (collision.transform.position - transform.position).normalized;
            direction.y = 0;
            SoundsManager.Instance.PlaySound(SoundType.MouseHit);
            collision.rigidbody.AddForce(direction * _pushForce, _pushForceMode);
        }
    }

    protected void StunRotate()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(transform.rotation.eulerAngles + _stunRotation, _stunRotationTime));
        sequence.Append(transform.DORotate(transform.rotation.eulerAngles, _stunRotationTime).SetDelay(_stunTime).OnComplete(OnStunEnd));
    }

    public virtual void OnStunEnd()
    {
        _isStunned = false;
    }

    public virtual void GetStunned()
    {
        _isStunned = true;
    }
}