public class NoTutorialWarningWindow : BaseWindow
{
    public void OnTutorialDeclineButton()
    {
        EventBus.OnTutorialDecline?.Invoke();
        WindowsManager.Instance.CloseCurrentWindow();
    }
}