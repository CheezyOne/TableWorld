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

        GameInfoHolder.Level++;
        WindowsManager.Instance.OpenWindow(_levelEndWindow);
        EventBus.OnGameEnd?.Invoke();
    }
}

public static class GameInfoHolder
{
    public static int Level;
    public static float CurrentHP = 100f;
}