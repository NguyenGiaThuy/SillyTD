using UnityEngine;

public class MissileLauncher : AttackTurret
{
    private void Awake()
    {
        ID = 1;
        canAntiAir = true;
        stats = Resources.Load<AttackTurretStats>("MissileLauncherData/MissileLauncherData" + level);
        LevelIncreased += MissileLauncher_LevelIncreased;
    }

    private void OnDestroy()
    {
        LevelIncreased -= MissileLauncher_LevelIncreased;
    }

    private void MissileLauncher_LevelIncreased()
    {
        stats = Resources.Load<AttackTurretStats>("MissileLauncherData/MissileLauncherData" + level);
    }
}
