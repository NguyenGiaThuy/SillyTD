using UnityEngine;

public class MissileLauncher : AttackTurret
{
    private void Awake()
    {
        ID = 1;
        canAntiAir = true;
        attackTurretStats = Resources.Load<AttackTurretStats>("MissileLauncherStats/MissileLauncherStats" + level);
        LevelIncreased += MissileLauncher_LevelIncreased;
    }

    private void OnDestroy()
    {
        LevelIncreased -= MissileLauncher_LevelIncreased;
    }

    private void MissileLauncher_LevelIncreased()
    {
        attackTurretStats = Resources.Load<AttackTurretStats>("MissileLauncherStats/MissileLauncherStats" + level);
    }
}
