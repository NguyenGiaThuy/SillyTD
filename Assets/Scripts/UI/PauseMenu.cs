using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenuPanel;

    private void Start()
    {
        pauseMenuPanel = transform.GetChild(0).gameObject;
        pauseMenuPanel.SetActive(false);
        GameManager.Instance.SubscribeToOnStateChanged(GameManager_OnStateChanged);
    }

    private void OnDestroy()
    {
        GameManager.Instance.UnsubscribeToOnStateChanged(GameManager_OnStateChanged);
    }

    private void GameManager_OnStateChanged(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.Pausing) pauseMenuPanel.SetActive(true);
        else pauseMenuPanel.SetActive(false);
    }

    public void Resume()
    {
        GameManager.Instance.UnPauseGame();
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
