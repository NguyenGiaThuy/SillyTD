using UnityEngine;

public class Artillery : AttackTurret
{
    private void Awake()
    {
        ID = 2;
        canAntiAir = false;
        turretStats = Resources.Load<TurretStats>("Artillery/ArtilleryStats" + level);
        OnLevelIncreased += Artillery_OnLevelIncreased;
    }

    private void OnDestroy()
    {
        OnLevelIncreased -= Artillery_OnLevelIncreased;
    }

    private void Artillery_OnLevelIncreased()
    {
        turretStats = Resources.Load<TurretStats>("Artillery/ArtilleryStats" + level);
        transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("Artillery/Material" + level);
        transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("Artillery/Material" + level);
        transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("Artillery/Material" + level);
    }
}
