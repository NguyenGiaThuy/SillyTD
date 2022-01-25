using UnityEngine;

public class Artillery : AttackTurret
{
    private void Awake()
    {
        ID = 2;
        canAntiAir = false;
        attackTurretStats = Resources.Load<AttackTurretStats>("ArtilleryStats/ArtilleryStats" + level);
        LevelIncreased += Artillery_LevelIncreased;
    }

    private void OnDestroy()
    {
        LevelIncreased -= Artillery_LevelIncreased;
    }

    private void Artillery_LevelIncreased()
    {
        attackTurretStats = Resources.Load<AttackTurretStats>("ArtilleryStats/ArtilleryStats" + level);
    }
}
