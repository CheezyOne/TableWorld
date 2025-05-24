using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;

public enum RewardType
{
    UnlimitedTime,
    AddBalloonUICell,
    ReturnBalloon,
}

public class AdsManager : SingletonDontDestroyOnLoad<AdsManager>
{
    [DllImport("__Internal")]
    private static extern void ShowInterAd();

    [DllImport("__Internal")]
    private static extern void ShowRVAd();
    [DllImport("__Internal")]
    private static extern void ShowBannerAd();

    [DllImport("__Internal")]
    private static extern void HideBannerAd();

    private const float FULLSCREEN_AD_DELAY = 33f;

    private bool canShowFullscreenAd = false;

    private Coroutine resetTimerCor;

    private RewardType _currentRewardType;

    public bool IsShowingBannerAd;

#if UNITY_EDITOR
    [SerializeField] private bool _isBannerAdTest;
    [SerializeField] private float _adInEditorTime;
#endif

    [SerializeField] private AudioSource[] _allAudioSources;
    private bool _isPausedByAd = false;

    private void Start()
    {
#if UNITY_EDITOR
        if (_isBannerAdTest)
        {
            IsShowingBannerAd = true;
            EventBus.OnBannerAdShown?.Invoke();
        }
#endif
        ResetTimer();
    }

    private void PauseAllAudio()
    {
        foreach (var audioSource in _allAudioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }

    private void UnpauseAllAudio()
    {
        foreach (var audioSource in _allAudioSources)
        {
            audioSource.UnPause();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && !_isPausedByAd)
        {
            StopGame();
            PauseAllAudio();
        }
        else if (hasFocus && !_isPausedByAd)
        {
            ResumeGame();
            UnpauseAllAudio();
        }
    }

    public void OnInterAdOpen()
    {
        _isPausedByAd = true;
        StopGame();
        PauseAllAudio();
    }

    public void OnInterAdClose()
    {
        _isPausedByAd = false;
        ResumeGame();
        UnpauseAllAudio();
        ResetTimer();
    }

    public void OnBannerAdShown()
    {
        IsShowingBannerAd = true;
        EventBus.OnBannerAdShown?.Invoke();
    }

    public void OnBannerAdHidden()
    {
        IsShowingBannerAd = false;
        EventBus.OnBannerAdHidden?.Invoke();
    }

    public void ShowRewarded(RewardType reward)
    {
        _currentRewardType = reward;

#if UNITY_EDITOR
        StartCoroutine(SimulateRewardedAd());
#endif
        if (!IsWebGL())
            return;

        ShowRVAd();
    }

    public void ShowInter()
    {
#if UNITY_EDITOR
        StartCoroutine(SimulateInterAd());
#endif
        if (!IsWebGL())
            return;

        if (canShowFullscreenAd)
        {
            ShowInterAd();
        }
    }

    private void ResetTimer()
    {
        if (resetTimerCor != null)
        {
            StopCoroutine(resetTimerCor);
            resetTimerCor = null;
        }
        resetTimerCor = StartCoroutine(ResetTimerIenum());
    }

    IEnumerator ResetTimerIenum()
    {
        canShowFullscreenAd = false;
        Debug.Log("CANNOT SHOW AD");
        yield return new WaitForSecondsRealtime(FULLSCREEN_AD_DELAY);
        canShowFullscreenAd = true;
        Debug.Log("CAN SHOW AD");
    }

#if UNITY_EDITOR
    private IEnumerator SimulateInterAd()
    {
        Debug.Log("Showing inter ad");
        StopGame();
        PauseAllAudio();
        yield return new WaitForSecondsRealtime(_adInEditorTime);
        ResumeGame();
        UnpauseAllAudio();
        Debug.Log("Inter ad shown");
    }

    private IEnumerator SimulateRewardedAd()
    {
        Debug.Log("Showing rewarded ad");
        StopGame();
        yield return new WaitForSecondsRealtime(_adInEditorTime);
        ResumeGame();
        GetReward();
        Debug.Log("Rewarded ad shown");
    }
#endif
    public void GetReward()
    {
        switch (_currentRewardType)
        {
            case RewardType.UnlimitedTime:
                {
                    break;
                }
            case RewardType.AddBalloonUICell:
                {
                    break;
                }
            case RewardType.ReturnBalloon:
                {
                    break;
                }
        }
        ResetTimer();
    }

    public void StopGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public static bool IsWebGL()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            return true;
#endif
        return false;
    }
}