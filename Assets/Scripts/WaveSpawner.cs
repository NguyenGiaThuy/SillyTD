using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    // Track the numbers of spawners
    public static int Counter { get; private set; } = 0;
    // Track current countdown of all spawners (shared between spawners)
    static public int CountDown { get; set; }

    public delegate void OnStateChangedHandler(WaveSpawnerState waveSpawnerState);
    public event OnStateChangedHandler StateChanged;

    public enum WaveSpawnerState
    {
        Null,
        Inactive,
        Active
    }

    public WaveSpawnerState CurrentState { get; private set; }
    public int CurrentWaveIndex { get; set; }

    [Header("Game Specifications", order = 0)]
    [SerializeField]
    private float timeBetweenWaves;
    [SerializeField]
    private float timeBetweenGroups;
    [SerializeField]
    private float timeBetweenUnits;
    [SerializeField]
    private int unitsPerWave;
    [SerializeField]
    private int unitsPerGroup;
    
    [Header("Unity Specifications", order = 0)]
    [SerializeField]
    private GameObject[] mobPrefabs;
    [SerializeField]
    private WayPoints wayPoints;
    [SerializeField]
    private int autoSaveAtWaveIndex;

    private void Awake()
    {
        GameManager.Instance.SubscribeToGameStateChanged(GameManager_StateChanged);
    }

    private void Start()
    {
        CurrentWaveIndex = 0;
    }

    private void OnDestroy()
    {
        GameManager.Instance.UnsubscribeToGameStateChanged(GameManager_StateChanged);
    }

    public void SetNewState(WaveSpawnerState newWaveSpawnerState)
    {
        if (CurrentState == newWaveSpawnerState) return;

        CurrentState = newWaveSpawnerState;
        StateChanged?.Invoke(newWaveSpawnerState);
    }

    private IEnumerator Spawn()
    {
        Counter++;
        CountDown = Mathf.RoundToInt(timeBetweenWaves);
        SetNewState(WaveSpawnerState.Active);

        // Begin spawning
        while (CurrentWaveIndex < mobPrefabs.Length)
        {
            // Spawn mobs according to timing
            int currentUnitsPerWave = unitsPerWave;
            while (currentUnitsPerWave > 0)
            {
                // Spawn and notify to subscribers
                Mob spawnedMob = Instantiate(mobPrefabs[CurrentWaveIndex], transform.position, transform.rotation).GetComponent<Mob>();
                spawnedMob.SetPath(wayPoints);
                currentUnitsPerWave--;

                if (currentUnitsPerWave % unitsPerGroup == 0) 
                    yield return new WaitForSeconds(timeBetweenGroups);
                else 
                    yield return new WaitForSeconds(timeBetweenUnits);
            }

            // Wait for the last mob to disappear
             yield return new WaitWhile(() => { return Mob.Counter != 0; });

            // Auto-save
            if (CurrentWaveIndex == autoSaveAtWaveIndex) GameManager.Instance.SetNewState(GameStateManager.GameState.Saving);

            // End coroutine if the last wave ends
            CurrentWaveIndex++;
            if (CurrentWaveIndex == mobPrefabs.Length) 
            {
                Counter--;
                SetNewState(WaveSpawnerState.Inactive);
                yield break; 
            }

            // Countdown next wave
            while (CountDown > 0)
            {
                yield return new WaitForSeconds(1f);
                CountDown--;
            }
        }
    }

    
    private void GameManager_StateChanged(GameStateManager.GameState gameState)
    {
        // Only called once at the beginning of the game
        if (gameState == GameStateManager.GameState.Playing && CurrentState != WaveSpawnerState.Active) StartCoroutine(Spawn());
    }
}
