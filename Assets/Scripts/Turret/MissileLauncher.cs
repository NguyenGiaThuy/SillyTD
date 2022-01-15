using UnityEngine;

public class MissileLauncher : AttackTurret
{
    private void Awake()
    {
        ID = 1;
        canAntiAir = true;
        data = Resources.Load<AttackTurretData>("MissileLauncherData/MissileLauncherData" + level);
        LevelIncreased += MissileLauncher_LevelIncreased;
    }

    private void OnDestroy()
    {
        LevelIncreased -= MissileLauncher_LevelIncreased;
    }

    private void MissileLauncher_LevelIncreased()
    {
        data = Resources.Load<AttackTurretData>("MissileLauncherData/MissileLauncherData" + level);
    }
}
