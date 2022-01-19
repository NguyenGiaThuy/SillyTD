using UnityEngine;

public class Artillery : AttackTurret
{
    private void Awake()
    {
        ID = 2;
        canAntiAir = false;
        data = Resources.Load<AttackTurretData>("ArtilleryData/ArtilleryData" + level);
        LevelIncreased += Artillery_LevelIncreased;
    }

    private void OnDestroy()
    {
        LevelIncreased -= Artillery_LevelIncreased;
    }

    private void Artillery_LevelIncreased()
    {
        data = Resources.Load<AttackTurretData>("ArtilleryData/ArtilleryData" + level);
    }
}
