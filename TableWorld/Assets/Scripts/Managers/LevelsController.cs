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
    public static int Decoys = 10;
    public static int Level;
    public static float CurrentHP = 100f;
    public static bool SoundOn = true;
    public static bool MusicOn = true;

    public static void ResetData()
    {
        Decoys = 10;
        Level = 0;
        CurrentHP = 100f;
    }
}