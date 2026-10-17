using UnityEngine;

public static class Utilities
{
    public enum GameState
    {
        Play,
        Pause
    }

    public static GameState State = GameState.Play;
}