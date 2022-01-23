using UnityEngine;

public class Artillery : AttackTurret
{
    private void Awake()
    {
        ID = 2;
        canAntiAir = false;
        stats = Resources.Load<AttackTurretStats>("ArtilleryData/ArtilleryData" + level);
        LevelIncreased += Artillery_LevelIncreased;
    }

    private void OnDestroy()
    {
        LevelIncreased -= Artillery_LevelIncreased;
    }

    private void Artillery_LevelIncreased()
    {
        stats = Resources.Load<AttackTurretStats>("ArtilleryData/ArtilleryData" + level);
    }
}
