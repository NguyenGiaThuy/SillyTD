using System.Collections;
using UnityEngine;

public class MobsPath : MonoBehaviour
{
    [Header("Unity Objects")]
    [SerializeField]
    private GameObject spawner;
    [SerializeField]
    private GameObject wayPoints;

    private void Awake()
    {
        spawner.GetComponent<Spawner>().OnMobSpawned += SetPath;
    }

    void SetPath()
    {
        spawner.GetComponent<Spawner>().GetSpawnedMob().SetPath(wayPoints.GetComponent<WayPoints>());
    }

    ~MobsPath()
    {
        spawner.GetComponent<Spawner>().OnMobSpawned -= SetPath;
    }
}
