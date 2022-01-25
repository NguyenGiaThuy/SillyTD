using UnityEngine;

public class StandardTurret : AttackTurret
{
    private void Awake()
    {
        ID = 0;
        canAntiAir = true;
        attackTurretStats = Resources.Load<AttackTurretStats>("StandardTurretStats/StandardTurretStats" + level);
        LevelIncreased += StandardTurret_LevelIncreased;
    }

    private void OnDestroy()
    {
        LevelIncreased -= StandardTurret_LevelIncreased;
    }

    private void StandardTurret_LevelIncreased()
    {
        attackTurretStats = Resources.Load<AttackTurretStats>("StandardTurretStats/StandardTurretStats" + level);
    }
}
