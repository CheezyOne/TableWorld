using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NormalMouse : Mouse
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _minRandomTargetTime;
    [SerializeField] private float _maxRandomTargetTime;
    [SerializeField] private float _randomTargetRadius;
    [SerializeField] private float _targetPlayerTime;
    [SerializeField] private float _restingTime;

    private Transform _player;
    private Transform _currentTarget;
    private bool _isStunned;
    private bool _isResting;
    private Coroutine _targetPlayerRoutine;

    private void Awake()
    {
        StartCoroutine(GetRandomTargetRoutine());
    }

    private void Update()
    {
        if (_currentTarget == null || _isStunned || _isResting)
        {
            if(_agent.isActiveAndEnabled)
                _agent.isStopped = true;

            return;
        }

        _agent.isStopped = false;
        _agent.destination = _currentTarget.position;
    }

    private IEnumerator GetRandomTargetRoutine()
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

    public void SetTemporalTarget(Transform target)
    {
        if (_currentTarget != _player)
            Destroy(_currentTarget.gameObject);

        if(_isResting)
        {
            Destroy(target.gameObject);
            return;
        }

        _currentTarget = target;

        if (_targetPlayerRoutine != null)
            StopCoroutine(_targetPlayerRoutine);

        _targetPlayerRoutine = StartCoroutine(ResetToTargetPlayer());
    }

    private IEnumerator ResetToTargetPlayer()
    {
        yield return new WaitForSeconds(_targetPlayerTime);
        Destroy(_currentTarget.gameObject);
        _currentTarget = _player;
    }

    public override void GetStunned()
    {
        _isStunned = true;
        _agent.enabled = false;
        StunRotate();
    }

    public override void OnStunEnd()
    {
        _agent.enabled = true;
        _isStunned = false;
    }
}