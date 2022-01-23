using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public void StartWave()
    {
        GameManager.Instance.SetNewState(GameStateManager.GameState.Playing);
    }

    public void Save()
    {
        GameManager.Instance.SetNewState(GameStateManager.GameState.Saving);
    }
}
