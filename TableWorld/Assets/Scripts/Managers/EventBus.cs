using System;

public static class EventBus
{
    public static Action OnBannerAdShown;
    public static Action OnBannerAdHidden;

    public static Action OnStomp;
    public static Action OnGameEnd;
    public static Action OnWindowClosed;

    public static Action OnSoundsToggle;
    public static Action OnMusicToggle;

    public static Action OnMenuCupHit;
    public static Action OnTutorialDecline;
}