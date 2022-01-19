using UnityEngine;

public class StandardTurret : AttackTurret
{
    private void Awake()
    {
        ID = 0;
        canAntiAir = true;
        data = Resources.Load<AttackTurretData>("StandardTurretData/StandardTurretData" + level);
        LevelIncreased += StandardTurret_LevelIncreased;
    }

    private void OnDestroy()
    {
        LevelIncreased -= StandardTurret_LevelIncreased;
    }

    private void StandardTurret_LevelIncreased()
    {
        data = Resources.Load<AttackTurretData>("StandardTurretData/StandardTurretData" + level);
    }
}
