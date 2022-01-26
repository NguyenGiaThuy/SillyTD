using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopBar : MonoBehaviour
{
    [SerializeField]
    private Image topBarMaskGlow;
    [SerializeField]
    private Button startWaveButton;
    [SerializeField]
    private TMP_Text totalWavesText;
    [SerializeField]
    private TMP_Text livesText;
    [SerializeField]
    private TMP_Text creditsText;

    private float topBarMaskGlowInitialWidth;

    private void Start()
    {
        topBarMaskGlowInitialWidth = topBarMaskGlow.rectTransform.rect.width;
        WaveManager.OnStateChanged += WaveManager_OnStateChanged;
        GameManager.Instance.OnLevelInitialized += Instance_OnLevelInitialized;
        totalWavesText.text = string.Concat(new string[] { (WaveManager.Instance.CurrentWaveIndex + 1).ToString(), "/", WaveManager.Instance.totalWaves.ToString() });
        livesText.text = GameManager.Instance.playerStats.lives.ToString();
        creditsText.text = GameManager.Instance.playerStats.credits.ToString();
    }

    private void OnDestroy()
    {
        WaveManager.OnStateChanged -= WaveManager_OnStateChanged;
        GameManager.Instance.OnLevelInitialized -= Instance_OnLevelInitialized;
    }

    private void Instance_OnLevelInitialized()
    {
        totalWavesText.text = string.Concat(new string[] { (WaveManager.Instance.CurrentWaveIndex + 1).ToString(), "/", WaveManager.Instance.totalWaves.ToString() });
    }

    private void WaveManager_OnStateChanged(WaveManager.WaveState waveState)
    {
        switch(waveState)
        {
            case WaveManager.WaveState.Started:
                startWaveButton.interactable = false;
                totalWavesText.text = string.Concat(new string[] 
                { (WaveManager.Instance.CurrentWaveIndex + 1).ToString(), "/", WaveManager.Instance.totalWaves.ToString() });
                break;
            default:
                startWaveButton.interactable = true;
                break;
        }
    }

    private void Update()
    {
        topBarMaskGlow.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 
            topBarMaskGlowInitialWidth * (WaveManager.Instance.WaveBreakCountdown / WaveManager.Instance.waveBreakDuration));
        livesText.text = GameManager.Instance.playerStats.lives.ToString();
        creditsText.text = GameManager.Instance.playerStats.credits.ToString();
    }

    public void StartWave()
    {
        GameManager.Instance.SetNewState(GameStateManager.GameState.Playing);
        WaveManager.Instance.SetNewState(WaveManager.WaveState.Started);
    }
}
