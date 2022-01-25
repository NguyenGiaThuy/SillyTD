using UnityEngine;

public class GameStateManager
{
    public delegate void OnStateChangedHandler(GameState gameState);
    public event OnStateChangedHandler OnStateChanged;

    public enum GameState
    {
        MainMenu,
        New,
        Resuming,
        Initializing,
        Preparing,
        Playing,
        Pausing,
        Victorious,
        Lost
    }

    public GameState CurrentState { get; private set; }

    public delegate void OnLostHandler();
    public event OnLostHandler Lost;
    public delegate void OnVictoriousHandler();
    public event OnVictoriousHandler Victorious;

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
