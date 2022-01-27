using UnityEngine;

public class MissileLauncher : AttackTurret
{
    private void Awake()
    {
        ID = 1;
        canAntiAir = true;
        turretStats = ScriptableObject.CreateInstance<TurretStats>();
        turretStats.CopyFrom(Resources.Load<TurretStats>("MissileLauncher/MissileLauncherStats" + level));
        OnLevelIncreased += MissileLauncher_OnLevelIncreased;
    }

    private void OnDestroy()
    {
        OnLevelIncreased -= MissileLauncher_OnLevelIncreased;
    }

    private void MissileLauncher_OnLevelIncreased()
    {
        turretStats.CopyFrom(Resources.Load<TurretStats>("MissileLauncher/MissileLauncherStats" + level));
        transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("MissileLauncher/Material" + level);
        transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("MissileLauncher/Material" + level);
        transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("MissileLauncher/Material" + level);
    }
}
