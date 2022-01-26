using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public int CurrentWaveIndex { get; set; }
    public float WaveBreakCountdown { get; private set; }
    

    public delegate void OnStateChangedHandler(WaveState waveState);
    public static event OnStateChangedHandler OnStateChanged;

    public enum WaveState
    {
        NotStarted,
        Started,
        Ended,
        BreakEnded,
        AllEnded
    }

    public WaveState CurrentState { get; private set; }

    public int totalWaves;
    public float waveDuration;
    public float waveBreakDuration;

    private void Awake()
    {
        Instance = this;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        GameManager.Instance.OnLevelInitialized += Instance_OnLevelInitialized;
        OnStateChanged += WaveManager_OnStateChanged;
        WaveBreakCountdown = waveBreakDuration;
        CurrentWaveIndex = 0;
        CurrentState = WaveState.NotStarted;
    }

    private void SceneManager_sceneUnloaded(Scene arg0)
    { 
        GameManager.Instance.OnLevelInitialized -= Instance_OnLevelInitialized;
        OnStateChanged -= WaveManager_OnStateChanged;
        SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
        Instance = null;
    }

    private void Instance_OnLevelInitialized()
    {
        StartCoroutine(BreakWave());
    }

    private void WaveManager_OnStateChanged(WaveState waveState)
    { 
        switch(waveState)
        {
            case WaveState.Started:
                StopAllCoroutines();
                WaveBreakCountdown = waveBreakDuration;
                break;
            case WaveState.Ended:
                CurrentWaveIndex++;
                if (CurrentWaveIndex >= totalWaves)
                {
                    SetNewState(WaveState.AllEnded);
                    break;
                }
                StartCoroutine(BreakWave());
                break;
            case WaveState.AllEnded:
                break;
        }
    }

    private IEnumerator BreakWave()
    {
        while (WaveBreakCountdown > 0)
        {
            yield return new WaitForSeconds(1f);
            WaveBreakCountdown -= 1;
        }

        WaveBreakCountdown = waveBreakDuration;
        SetNewState(WaveState.Started);
    }

    public void SetNewState(WaveState newWaveState)
    {
        if (newWaveState == CurrentState) return;

        CurrentState = newWaveState;
        OnStateChanged?.Invoke(newWaveState);
    }
}
