using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Resume()
    {
        GameManager.Instance.LoadSavedLevel();
    }

    public void NewGame()
    {
        GameManager.Instance.LoadNewLevel();
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
