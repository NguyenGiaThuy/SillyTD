using UnityEngine;

public class GameStateManager
{
    public delegate void OnStateChangedHandler(GameState gameState);
    public event OnStateChangedHandler StateChanged;

    public enum GameState
    {
        Null,
        Pausing,
        Initializing,
        Preparing,
        Playing,
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
        CurrentState = GameState.Null;
    }

    public void SetNewState(GameState newGameState)
    {
        if (newGameState == CurrentState) return;

        CurrentState = newGameState;
        StateChanged?.Invoke(newGameState);
    }
}
