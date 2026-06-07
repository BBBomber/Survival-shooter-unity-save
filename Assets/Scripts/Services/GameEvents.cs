using System;

//bare minimum event class
public static class GameEvents
{
    public static event Action PlayerDied;

    public static void RaisePlayerDied() => PlayerDied?.Invoke();
}