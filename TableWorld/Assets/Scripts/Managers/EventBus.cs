using System;

public static class EventBus
{
    public static Action OnStomp;
    public static Action OnGameEnd;
    public static Action OnWindowClosed;
}