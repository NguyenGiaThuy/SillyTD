using UnityEngine;

public class MissileLauncher : AttackTurret
{
    private void Awake()
    {
        id = 1;
        canAntiAir = true;
        turretParameter = ScriptableObject.CreateInstance<TurretParameter>();
        turretParameter.CopyFrom(Resources.Load<TurretParameter>("MissileLauncher/MissileLauncherStats" + levels));
        OnLevelIncreased += MissileLauncher_OnLevelIncreased;
    }

    private void OnDestroy()
    {
        OnLevelIncreased -= MissileLauncher_OnLevelIncreased;
    }

    private void MissileLauncher_OnLevelIncreased()
    {
        turretParameter.CopyFrom(Resources.Load<TurretParameter>("MissileLauncher/MissileLauncherStats" + levels));
        transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("MissileLauncher/Material" + levels);
        transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("MissileLauncher/Material" + levels);
        transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("MissileLauncher/Material" + levels);
    }
}
