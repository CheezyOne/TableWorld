using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NormalMouse : Mouse
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _minRandomTargetTime;
    [SerializeField] private float _maxRandomTargetTime;
    [SerializeField] private float _randomTargetRadius;
    [SerializeField] private float _targetPlayerTime;
    [SerializeField] private float _restingTime;

    private Transform _player;
    private Transform _currentTarget;
    private bool _isResting;
    private bool _isEating;
    private Coroutine _targetPlayerRoutine;
    private Coroutine _restingRoutine;

    public bool IsEating => _isEating;

    private void Awake()
    {
        _restingRoutine = StartCoroutine(RestingRoutine());
    }

    private void Update()
    {
        if (_currentTarget == null || _isStunned)
        {
            if(_agent.isActiveAndEnabled)
                _agent.isStopped = true;

            return;
        }

        _agent.isStopped = false;
        
        if (_isResting)
            return;

        _agent.destination = _currentTarget.position;
    }

    private IEnumerator RestingRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minRandomTargetTime, _maxRandomTargetTime));
            
            if (_isStunned)
                continue;

            _isResting = true;
            _agent.destination = transform.position + new Vector3(Random.insideUnitCircle.x * _randomTargetRadius, 0, Random.insideUnitSphere.z * _randomTargetRadius);
            yield return new WaitForSeconds(_restingTime);
            _isResting = false;
        }
    }

    public void SetPlayer(Transform player)
    {
        _player = player;
        _currentTarget = _player;
    }

    public void SetDecoyTarget(Transform target)
    {
        if (_currentTarget != _player)
            Destroy(_currentTarget.gameObject);

        if(_isResting)
        {
            _currentTarget = _player;
            _isResting = false;
        }

        StopCoroutine(_restingRoutine);
        _currentTarget = target;
        _isEating = true;

        if (_targetPlayerRoutine != null)
            StopCoroutine(_targetPlayerRoutine);

        _targetPlayerRoutine = StartCoroutine(TargetPlayerRoutine());
    }

    private IEnumerator TargetPlayerRoutine()
    {
        yield return new WaitForSeconds(_targetPlayerTime);
        _restingRoutine = StartCoroutine(RestingRoutine());
        _isEating = false;
        Destroy(_currentTarget.gameObject);
        _currentTarget = _player;
    }

    public override void GetStunned()
    {
        base.GetStunned();
        _animator.enabled = false;
        _agent.enabled = false;
        StunRotate();
    }

    public override void OnStunEnd()
    {
        base.OnStunEnd();
        _animator.enabled = true;
        _agent.enabled = true;
    }
}