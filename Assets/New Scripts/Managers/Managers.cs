using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TurretManager))]
public class Managers : MonoBehaviour
{
    public static TurretManager turretManager { get; private set; }

    private List<IGameManager> managersStartSequence;

    private void Awake()
    {
        turretManager = GetComponent<TurretManager>();

        managersStartSequence = new List<IGameManager>();
        managersStartSequence.Add(turretManager);

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        foreach (IGameManager manager in managersStartSequence) manager.Startup();

        yield return null;

        int modulesCount = managersStartSequence.Count;
        int readyCount = 0;

        while(readyCount < modulesCount)
        {
            int lastReady = readyCount;
            readyCount = 0;

            foreach (IGameManager manager in managersStartSequence)
                if (manager.status == ManagerStatus.Started) readyCount++;

            if (readyCount > lastReady) Debug.Log($"Progress: {readyCount}/{modulesCount}");
            yield return null;
        }

        Debug.Log("All managers started up");
    }
}
