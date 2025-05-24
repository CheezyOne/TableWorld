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

        SoundsManager.Instance.PlaySound(SoundType.Victory);
        SaveLoadSystem.data.Level++;
        SaveLoadSystem.Instance.Save();
        WindowsManager.Instance.OpenWindow(_levelEndWindow);
        EventBus.OnGameEnd?.Invoke();
    }
}

public static class GameInfoHolder
{

}