using System;

[Serializable]
public class GameManagerData 
{
    GameStateManager.GameState gameState;
    int levelIndex;

    public void Save(GameManager gameManager)
    {
        gameState = gameManager.previousGameState;
        levelIndex = gameManager.levelIndex;
    }

    public void Load(GameManager gameManager)
    {
        gameManager.savedState = gameState;
        gameManager.levelIndex = levelIndex;
    }
}
