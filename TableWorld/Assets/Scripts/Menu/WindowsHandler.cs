using UnityEngine;

public class WindowsHandler : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private StartingSequence _startingSequence;

    private void OpenMenu()
    {
        if (_startingSequence.IsInAction)
            return;

        _menu.SetActive(true);
    }

    private void OnEnable()
    {
        EventBus.OnWindowClosed += OpenMenu;
    }

    private void OnDisable()
    {
        EventBus.OnWindowClosed -= OpenMenu;
    }
}