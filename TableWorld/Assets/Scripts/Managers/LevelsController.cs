using UnityEngine;

public class LevelsController : Singleton<LevelsController>
{
    [SerializeField] private LevelEndWindow _levelEndWindow;

    public void CompleteLevel()
    {
        if (WindowsManager.Instance.IsOpened(typeof(LevelEndWindow)))
            return;

#if UNITY_EDITOR
        Debug.Log("Level ended");
#endif

        if (SaveLoadSystem.data.Level == 0)
            GameAnalytics.TrackEvent("first_level_complete");
        else if (SaveLoadSystem.data.Level == 1)
            GameAnalytics.TrackEvent("second_level_complete");
        else if (SaveLoadSystem.data.Level == 2)
            GameAnalytics.TrackEvent("third_level_complete");

        SoundsManager.Instance.PlaySound(SoundType.Victory);
        SaveLoadSystem.data.Level++;
        SaveLoadSystem.Instance.Save();
        WindowsManager.Instance.OpenWindow(_levelEndWindow);
        EventBus.OnGameEnd?.Invoke();
    }
}