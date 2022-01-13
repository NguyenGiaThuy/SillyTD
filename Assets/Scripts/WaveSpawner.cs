using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    // Track the numbers of spawners
    public static int Counter { get; private set; } = 0;
    // Track current countdown of all spawners (shared between spawners)
    static public int CountDown { get; set; }

    public delegate void OnStateChangedHandler(WaveSpawnerState gameState);
    public event OnStateChangedHandler StateChanged;

    public enum WaveSpawnerState
    {
        Null,
        Inactive,
        Active
    }

    public WaveSpawnerState State { get; private set; }
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

    private void Awake()
    {
        GameManager.gameManager.StateChanged += GameManager_StateChanged;
    }

    private void Start()
    {
        Load(0);
    }

    private void OnDestroy()
    {
        GameManager.gameManager.StateChanged -= GameManager_StateChanged;
    }

    // Used for loading level
    public void Load(int currentWaveIndex)
    {
        CurrentWaveIndex = currentWaveIndex;
    }

    public void SetState(WaveSpawnerState waveSpawnerState)
    {
        if (State == waveSpawnerState) return;

        State = waveSpawnerState;
        StateChanged?.Invoke(waveSpawnerState);
    }

    private IEnumerator Spawn()
    {
        Counter++;
        CountDown = Mathf.RoundToInt(timeBetweenWaves);
        SetState(WaveSpawnerState.Active);

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

            // End coroutine if the last wave ends
            CurrentWaveIndex++;
            if (CurrentWaveIndex == mobPrefabs.Length) 
            {
                Counter--;
                SetState(WaveSpawnerState.Inactive);
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

    // Only called once at the beginning of the game
    private void GameManager_StateChanged(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.Playing && State != WaveSpawnerState.Active) StartCoroutine(Spawn());
    }
}
