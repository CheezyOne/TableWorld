using UnityEngine;

public class PauseWindow : BaseWindow
{
    public override void Init()
    {
        base.Init();
        Time.timeScale = 0f;
    }

    public override void OnClose()
    {
        Time.timeScale = 1f;
    }
}