using UnityEngine;
using TMPro;
using System.Threading;

public class TimerController : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;

    private float _timeLeft;
    private bool _isPlaying;

    public void StartTimer(float time)
    {
        _timeLeft = time;
        _isPlaying = true;
    }

    private void Update()
    {
        if (!_isPlaying)
            return;

        _timeLeft -= Time.deltaTime;

        if (_timeLeft <= 0)
        {
            EventBus.OnTimeOut?.Invoke();
            _isPlaying = false;
        }
    }
}