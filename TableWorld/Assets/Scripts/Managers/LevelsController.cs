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

        if (SaveLoadSystem.data.Level == 0 && !SaveLoadSystem.data.IsFirstLevelComplete)
        {
            GameAnalytics.Instance.TrackEvent("first_level_complete");
            SaveLoadSystem.data.IsFirstLevelComplete = true;
        }
        else if (SaveLoadSystem.data.Level == 1 && !SaveLoadSystem.data.IsSecondLevelComplete)
        {
            GameAnalytics.Instance.TrackEvent("second_level_complete");
            SaveLoadSystem.data.IsSecondLevelComplete = true;
        }
        else if (SaveLoadSystem.data.Level == 2 && !SaveLoadSystem.data.IsThirdLevelComplete)
        {
            GameAnalytics.Instance.TrackEvent("third_level_complete");
            SaveLoadSystem.data.IsThirdLevelComplete = true;
        }

        SoundsManager.Instance.PlaySound(SoundType.Victory);
        SaveLoadSystem.data.Level++;
        SaveLoadSystem.Instance.Save();
        WindowsManager.Instance.OpenWindow(_levelEndWindow);
        EventBus.OnGameEnd?.Invoke();
    }
}