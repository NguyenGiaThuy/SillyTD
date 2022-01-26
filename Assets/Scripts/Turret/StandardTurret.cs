using UnityEngine;

public class StandardTurret : AttackTurret
{
    private void Awake()
    {
        ID = 0;
        canAntiAir = true;
        turretStats = Resources.Load<TurretStats>("StandardTurret/StandardTurretStats" + level);
        OnLevelIncreased += StandardTurret_OnLevelIncreased;
    }

    private void OnDestroy()
    {
        OnLevelIncreased -= StandardTurret_OnLevelIncreased;
    }

    private void StandardTurret_OnLevelIncreased()
    {
        turretStats = Resources.Load<TurretStats>("StandardTurret/StandardTurretStats" + level);
        transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("StandardTurret/Material" + level);
        transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("StandardTurret/Material" + level);
        transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("StandardTurret/Material" + level);
    }
}
