using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MouseTrap : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _afterHitTime;
    [SerializeField] private float _yDistanceMovement;
    [SerializeField] private float _yMovementTime;

    private bool _isActivated;

    private void OnTriggerEnter(Collider other)
    {
        if (_isActivated)
            return;

        if(other.TryGetComponent(out PlayerHealth playerHealth))
        {
            _isActivated = true;
            playerHealth.TakeDamage(_damage);
            //Play animation and delete self
        }
        else if (other.TryGetComponent(out Mouse mouse))
        {
            _isActivated = true;
            mouse.GetStunned();
            //Play animation and delete self
        }
    }

    public void StartAfterHitAnimation()
    {
        StartCoroutine(DestoyAnimation());
    }

    private IEnumerator DestoyAnimation()
    {
        yield return new WaitForSeconds(_afterHitTime);
        transform.DOMoveY(-_yDistanceMovement, _yMovementTime).OnComplete(() => Destroy(gameObject));
    }
}