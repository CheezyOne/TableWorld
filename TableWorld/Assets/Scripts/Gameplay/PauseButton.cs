using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private PauseWindow _pauseWindow;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !WindowsManager.Instance.IsOpened(typeof(PauseWindow)))
            OpenPauseWindow();
    }

    public void OpenPauseWindow()
    {
        WindowsManager.Instance.OpenWindow(_pauseWindow);
    }
}