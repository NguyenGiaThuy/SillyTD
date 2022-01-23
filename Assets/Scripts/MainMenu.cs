using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Resume()
    {
        GameManager.Instance.SetNewState(GameStateManager.GameState.Resuming);
    }

    public void NewGame()
    {
        GameManager.Instance.SetNewState(GameStateManager.GameState.New);
    }

    public void Glossary()
    {

    }

    public void Settings()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
