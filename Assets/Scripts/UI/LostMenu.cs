using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostMenu : MonoBehaviour
{
    private GameObject lostMenuPanel;

    private void Start()
    {
        lostMenuPanel = transform.GetChild(0).gameObject;
        lostMenuPanel.SetActive(false);
        GameManager.Instance.SubscribeToOnStateChanged(GameManager_OnStateChanged);
    }

    private void OnDestroy()
    {
        GameManager.Instance.UnsubscribeToOnStateChanged(GameManager_OnStateChanged);
    }

    private void GameManager_OnStateChanged(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.Lost)
        {
            lostMenuPanel.SetActive(true);
            GameManager.Instance.PauseGame(false);
        }
    }

    public void ResumeRecent()
    {
        GameManager.Instance.UnPauseGame(false);
        GameManager.Instance.LoadSavedLevel();
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
