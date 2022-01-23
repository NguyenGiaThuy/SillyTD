using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CustomInputActions inputActions;

    private void Awake()
    {
        inputActions = new CustomInputActions();
        GameManager.Instance.SubscribeToGameStateChanged(GameStateManager_OnStateChanged);
        inputActions.Gameplay.PauseGame.performed += PauseGame_performed;
        inputActions.NodeUI.CancelNodeSelection.performed += CancelNodeSelection_performed;
    }

    private void CancelNodeSelection_performed(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.CurrentState == GameStateManager.GameState.Preparing ||
            GameManager.Instance.CurrentState == GameStateManager.GameState.Playing)
            FindObjectOfType<NodeUI>().HidePanel();
    }

    private void PauseGame_performed(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.CurrentState != GameStateManager.GameState.Pausing) GameManager.Instance.SetNewState(GameStateManager.GameState.Pausing);
        else GameManager.Instance.SetNewState(GameStateManager.GameState.Playing);
    }

    private void GameStateManager_OnStateChanged(GameStateManager.GameState gameState)
    {
        switch(gameState)
        {
            case GameStateManager.GameState.Playing:
                inputActions.Enable();
                break;
            case GameStateManager.GameState.Pausing:
                inputActions.Enable();
                inputActions.NodeUI.CancelNodeSelection.Disable();
                break;
            case GameStateManager.GameState.Preparing:
                inputActions.Enable();
                break;
            default:
                inputActions.Disable();
                break;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.UnsubscribeToGameStateChanged(GameStateManager_OnStateChanged);
    }

}
