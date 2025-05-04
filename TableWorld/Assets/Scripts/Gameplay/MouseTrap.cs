using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MouseTrap : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _afterHitTime;
    [SerializeField] private float _yDistanceMovement;
    [SerializeField] private float _yMovementTime;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animationName = "Scene";
    [SerializeField] private bool _disableAfterPlay = true;

    private bool _isActivated;

    private void Awake()
    {
        _animator.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActivated)
            return;

        if(other.TryGetComponent(out PlayerHealth playerHealth))
        {
            _isActivated = true;
            playerHealth.TakeDamage(_damage);
            PlayAnimation();
        }
        else if (other.TryGetComponent(out Mouse mouse))
        {
            SoundsManager.Instance.PlaySound(SoundType.MouseGotHit);
            _isActivated = true;
            mouse.GetStunned();
            PlayAnimation();
        }
    }

    public void PlayAnimation()
    {
        if (!_animator.enabled)
            _animator.enabled = true;

        SoundsManager.Instance.PlaySound(SoundType.MousetrapHit);
        _animator.Play(_animationName, -1, 0f);
        StartCoroutine(WaitForAnimationComplete());
    }

    private IEnumerator WaitForAnimationComplete()
    {
        yield return null;
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        OnAnimationComplete();
    }

    private void OnAnimationComplete()
    {
        if (_disableAfterPlay)
            _animator.enabled = false;

        StartAfterHitAnimation();
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