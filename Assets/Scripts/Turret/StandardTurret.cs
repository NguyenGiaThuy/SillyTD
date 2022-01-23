using UnityEngine;

public class StandardTurret : AttackTurret
{
    private void Awake()
    {
        ID = 0;
        canAntiAir = true;
        stats = Resources.Load<AttackTurretStats>("StandardTurretData/StandardTurretData" + level);
        LevelIncreased += StandardTurret_LevelIncreased;
    }

    private void OnDestroy()
    {
        LevelIncreased -= StandardTurret_LevelIncreased;
    }

    private void StandardTurret_LevelIncreased()
    {
        stats = Resources.Load<AttackTurretStats>("StandardTurretData/StandardTurretData" + level);
    }
}
