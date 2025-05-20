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

    private const float FULLSCREEN_AD_DELAY = 33f;

    private bool canShowFullscreenAd = false;

    private Coroutine resetTimerCor;

    private RewardType _currentRewardType;

    public bool IsShowingBannerAd;

#if UNITY_EDITOR
    [SerializeField] private bool _isBannerAdTest;
    [SerializeField] private float _adInEditorTime;
#endif

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
        yield return new WaitForSecondsRealtime(_adInEditorTime);
        ResumeGame();
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

    public void OnInterAdOpen()
    {
        StopGame();
    }

    public void OnInterAdClose()
    {
        ResumeGame();
        ResetTimer();
    }

    public static bool IsWebGL()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            return true;
#endif
        return false;
    }
}