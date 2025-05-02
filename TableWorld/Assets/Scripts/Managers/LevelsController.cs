using UnityEngine;

public class LevelsController : Singleton<LevelsController>
{
    [SerializeField] private LevelEndWindow _levelEndWindow;

    public void CompleteLevel()
    {
#if UNITY_EDITOR
        Debug.Log("Level ended");
#endif

        GameInfoHolder.Level++;
        WindowsManager.Instance.OpenWindow(_levelEndWindow);
    }
}

public static class GameInfoHolder
{
    public static int Level;
}