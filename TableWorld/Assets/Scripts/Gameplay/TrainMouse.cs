using UnityEngine;
using DG.Tweening;

public class TrainMouse : Mouse
{
    [SerializeField] private float _travelTime;
    [SerializeField] private float _distanceToTravel;

    private void Awake()
    {
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
    }

    public override void OnStunEnd()
    {
        TrainRun();
    }
}