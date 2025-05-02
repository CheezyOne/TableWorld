using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Legs : MonoBehaviour
{
    [SerializeField] private float _minBetweenStompsTime;
    [SerializeField] private float _maxBetweenStompsTime;
    [SerializeField] private float _betweenStompsTimeReduction;
    [SerializeField] private float _absoluteLowestBetweenStompsTime;
    [SerializeField] private Transform _legs;
    [SerializeField] private Transform _upperStompingPoint;
    [SerializeField] private ShockWave _shockWave;
    [SerializeField] private float _liftingLegsTime;
    [SerializeField] private float _holdingLegsTime;
    [SerializeField] private float _stompingTime;

    private void Awake()
    {
        StartCoroutine(StompRoutine());
    }

    private IEnumerator StompRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(GetBetweenStompsTime());
            Stomp();
        }
    }

    private void Stomp()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_legs.DOMove(_upperStompingPoint.position, _liftingLegsTime));
        sequence.Append(_legs.DOMove(transform.position, _stompingTime).SetDelay(_holdingLegsTime).OnComplete(()=> OnStomp()));
    }

    private void OnStomp()
    {
        EventBus.OnStomp?.Invoke();
        _shockWave.StartWave();
    }

    private float GetBetweenStompsTime()
    {
        float reductedTime = _maxBetweenStompsTime - _betweenStompsTimeReduction * GameInfoHolder.Level;
        float betweenStompsTime = reductedTime<_minBetweenStompsTime?Random.Range(_absoluteLowestBetweenStompsTime, _minBetweenStompsTime) : Random.Range(_minBetweenStompsTime, reductedTime);
        return betweenStompsTime;
    }
}