using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static int Counter { get; private set; } = 0;

    [Header("Game Specifications", order = 0)]
    [SerializeField]
    private int spawnAtWave;
    [SerializeField]
    private int stopAtWave;
    [SerializeField]
    private float startAfter;
    [SerializeField]
    private int spawnFrequency;
    [SerializeField]
    private int unitsPerWave;
    [SerializeField]
    private int unitsPerGroup;

    [Header("Unity Specifications", order = 0)]
    [SerializeField]
    private GameObject mobPrefab;
    [SerializeField]
    private WayPoints wayPoints;

    private void Start()
    {
        WaveManager.OnStateChanged += WaveSpawner_OnWaveStateChangedChanged;
    }

    private void WaveSpawner_OnWaveStateChangedChanged(WaveManager.WaveState waveState)
    { 
        switch(waveState)
        {
            case WaveManager.WaveState.Started:
                if(spawnAtWave - 1 == WaveManager.Instance.CurrentWaveIndex || 
                    (WaveManager.Instance.CurrentWaveIndex != 0 
                    && WaveManager.Instance.CurrentWaveIndex % spawnFrequency == 0
                    && stopAtWave - 1 <= WaveManager.Instance.CurrentWaveIndex)) 
                    StartCoroutine(Spawn());
                break;
            case WaveManager.WaveState.Ended:
                break;
            case WaveManager.WaveState.AllEnded:
                break;
        }
    }

    private void OnDestroy()
    {
        WaveManager.OnStateChanged -= WaveSpawner_OnWaveStateChangedChanged;
    }

    // Spawn every wave
    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(startAfter);

        Counter++;

        float timeBetweenUnits = WaveManager.Instance.waveDuration / unitsPerWave;
        float timeBetweenUnitsInGroup = timeBetweenUnits / unitsPerGroup;
        float groupDuration = (unitsPerGroup - 1) * timeBetweenUnitsInGroup;
        int groupsPerWave = unitsPerWave / unitsPerGroup;
        float timeBetweenGroups = (WaveManager.Instance.waveDuration - groupDuration * groupsPerWave) / (groupsPerWave - 1);

        int currentUnitsPerWave = unitsPerWave;
        while (currentUnitsPerWave > 0)
        {
            // Spawn
            Mob spawnedMob = Instantiate(mobPrefab, transform.position, transform.rotation).GetComponent<Mob>();
            spawnedMob.SetPath(wayPoints);

            currentUnitsPerWave--;
            if(currentUnitsPerWave == 0) break;

            float timeToWait = timeBetweenUnitsInGroup;
            // Wait for groups/units to spawn
            if (currentUnitsPerWave % unitsPerGroup == 0) timeToWait = timeBetweenGroups;
            yield return new WaitForSeconds(timeToWait);
        }

        yield return new WaitUntil(() => { return Mob.Counter == 0; });
        Counter--;
        if (Counter == 0)
        {
            yield return new WaitForSeconds(1f);
            WaveManager.Instance.SetNewState(WaveManager.WaveState.Ended);
        }
    }
}
