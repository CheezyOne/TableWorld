public class NoTutorialWarningWindow : BaseWindow
{
    public void OnTutorialDeclineButton()
    {
        GameAnalytics.Instance.TrackEvent("decline_tutorial");
        EventBus.OnTutorialDecline?.Invoke();
        WindowsManager.Instance.CloseCurrentWindow();
    }
}