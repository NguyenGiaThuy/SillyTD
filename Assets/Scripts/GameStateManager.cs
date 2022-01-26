using UnityEngine;

public class GameStateManager
{
    public delegate void OnStateChangedHandler(GameState gameState);
    public event OnStateChangedHandler OnStateChanged;

    public enum GameState
    {
        MainMenu,
        Preparing,
        Playing,
        Pausing,
        Victorious,
        Lost
    }

    public GameState CurrentState { get; private set; }

    public GameStateManager()
    {
        CurrentState = GameState.MainMenu;
    }

    public void SetNewState(GameState newGameState)
    {
        if (newGameState == CurrentState) return;

        CurrentState = newGameState;
        OnStateChanged?.Invoke(newGameState);
    }
}
