using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Action OnMobSpawned;

    [Header("Specifications")]
    [SerializeField]
    private float spawnAfter;
    [SerializeField]
    private int mobsPerWave;
    [SerializeField]
    private float timeBetweenWaves;
    [SerializeField]
    private int mobsPerGroup;
    [SerializeField]
    private float timeBetweenGroups;
    [SerializeField]
    private float timeBetweenMobs;

    [Header("Unity Objects")]
    [SerializeField]
    private GameObject[] mobsToSpawn;

    private GameObject spawnedMob;
    private int nextMobWaves;

    private void Awake()
    {
        nextMobWaves = 0;
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        // Begin wave after
        yield return new WaitForSeconds(spawnAfter);

        // Begin spawning
        int tempNextMobWaves = nextMobWaves;
        while (tempNextMobWaves < mobsToSpawn.Length)
        {
            // Spawn mobs according to timing
            int tempMobsPerWave = mobsPerWave;
            while (tempMobsPerWave > 0)
            {
                // Spawn and notify to subscribers
                spawnedMob = Instantiate(mobsToSpawn[tempNextMobWaves], transform.position, transform.rotation);
                OnMobSpawned?.Invoke();
                tempMobsPerWave--;

                if (tempMobsPerWave % mobsPerGroup == 0)
                {
                    yield return new WaitForSeconds(timeBetweenGroups);
                }
                else
                {
                    yield return new WaitForSeconds(timeBetweenMobs);
                }
            }

            // Wait for the last mob to disappear
            while (FindObjectOfType<Mob>() != null)
            {
                yield return new WaitForSeconds(1f);
            }

            // End coroutine if the last wave ends
            tempNextMobWaves++;
            if (tempNextMobWaves == mobsToSpawn.Length)
            {
                yield break;
            }

            // Countdown next wave
            int countDown = Mathf.RoundToInt(timeBetweenWaves);
            while (countDown > 0)
            {
                yield return new WaitForSeconds(1f);
                countDown--;
            }
        }
    }

    public Mob GetSpawnedMob()
    {
        return spawnedMob.GetComponent<Mob>();
    }
}
