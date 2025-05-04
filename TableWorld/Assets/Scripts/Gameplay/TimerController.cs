using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class TimerController : Singleton<TimerController>
{
    [SerializeField] private ArmSpawner _armSpawner;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private string _runToHandText;
    [SerializeField] private float _timerScaleMultiplier;
    [SerializeField] private float _timerScaleTime;
    [SerializeField] private float _firstLevelTime;
    [SerializeField] private float _levelEncreaseTime;

    private float _timeLeft;
    private bool _isPlaying;

    public float TimeLeft => _timeLeft;

    protected override void Awake()
    {
        base.Awake();
        StartTimer(_firstLevelTime + GameInfoHolder.Level * _levelEncreaseTime);
    }

    private void StartTimer(float time)
    {
        _timeLeft = time;
        _isPlaying = true;
    }

    public float GetPassedTime()
    {
        return _firstLevelTime + GameInfoHolder.Level * _levelEncreaseTime - _timeLeft;
    }

    private void Update()
    {
        if (!_isPlaying)
            return;

        _timeLeft -= Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(_timeLeft);
        _timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        if (_timeLeft <= 0)
        {
            _armSpawner.SpawnArm();
            _isPlaying = false;
            _timerText.text = _runToHandText;
            _timerText.transform.DOScale(_timerScaleMultiplier, _timerScaleTime).SetLoops(-1, LoopType.Yoyo);
        }
    }
}