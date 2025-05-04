using UnityEngine;
using DG.Tweening;

public class TrainMouse : Mouse
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _travelTime;
    [SerializeField] private float _distanceToTravel;

    private void Awake()
    {
        SoundsManager.Instance.PlaySound(SoundType.TrainMouseSpawn);
        TrainRun();
    }

    private void TrainRun()
    {
        transform.DOMove(transform.position + transform.forward * _distanceToTravel, _travelTime).OnComplete(() => Destroy(gameObject));
    }

    public override void GetStunned()
    {
        transform.DOKill();
        StunRotate();
        _animator.enabled= false;
    }

    public override void OnStunEnd()
    {
        _animator.enabled = true;
        TrainRun();
    }
}