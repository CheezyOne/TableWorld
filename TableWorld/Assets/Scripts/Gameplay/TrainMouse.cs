using UnityEngine;
using DG.Tweening;

public class TrainMouse : Mouse
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _travelTime;
    [SerializeField] private float _distanceToTravel;
    [SerializeField] private ParticleSystem _rightTrail;
    [SerializeField] private ParticleSystem _leftTrail;
    [SerializeField] private float _standartDustEmission;

    private ParticleSystem.EmissionModule _rightTrailEmission;
    private ParticleSystem.EmissionModule _leftTrailEmission;

    private void Awake()
    {
        _leftTrailEmission = _leftTrail.emission;
        _rightTrailEmission = _rightTrail.emission;
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
        _rightTrailEmission.rateOverTime = 0;
        _leftTrailEmission.rateOverTime = 0;
        _animator.enabled= false;
    }

    public override void OnStunEnd()
    {
        _rightTrailEmission.rateOverTime = _standartDustEmission;
        _leftTrailEmission.rateOverTime = _standartDustEmission;
        _animator.enabled = true;
        TrainRun();
    }
}