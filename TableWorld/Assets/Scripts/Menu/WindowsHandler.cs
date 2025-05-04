using UnityEngine;

public class WindowsHandler : MonoBehaviour
{
    [SerializeField] private GameObject _menu;

    private void OpenMenu()
    {
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