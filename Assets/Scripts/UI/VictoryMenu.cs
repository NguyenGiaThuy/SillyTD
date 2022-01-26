using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryMenu : MonoBehaviour
{
    private GameObject victoryMenuPanel;

    private void Start()
    {
        victoryMenuPanel = transform.GetChild(0).gameObject;
        victoryMenuPanel.SetActive(false);
        GameManager.Instance.SubscribeToOnStateChanged(GameManager_OnStateChanged);
    }

    private void OnDestroy()
    {
        GameManager.Instance.UnsubscribeToOnStateChanged(GameManager_OnStateChanged);
    }

    private void GameManager_OnStateChanged(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.Victorious)
        {
            victoryMenuPanel.SetActive(true);
            GameManager.Instance.PauseGame(false);
        }
    }

    public void NextLevel()
    {
        GameManager.Instance.UnPauseGame(false);
        GameManager.Instance.LoadNextLevel();
    }

    public void Restart()
    {
        GameManager.Instance.UnPauseGame(false);
        GameManager.Instance.RestartLevel();
    }

    public void MainMenu()
    {
        GameManager.Instance.UnPauseGame(false);
        GameManager.Instance.LoadMainMenu();
    }

    public void Glossary()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
