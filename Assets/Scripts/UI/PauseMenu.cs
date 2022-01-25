using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenuPanel;

    private void Start()
    {
        pauseMenuPanel = transform.GetChild(0).gameObject;
        pauseMenuPanel.SetActive(false);
        GameManager.Instance.SubscribeToOnGameStateChanged(GameManager_OnStateChanged);
    }

    private void OnDestroy()
    {
        GameManager.Instance.UnsubscribeToOnGameStateChanged(GameManager_OnStateChanged);
    }

    private void GameManager_OnStateChanged(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.Pausing) pauseMenuPanel.SetActive(true);
        else pauseMenuPanel.SetActive(false);
    }
}
