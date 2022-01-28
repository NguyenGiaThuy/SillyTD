using UnityEngine;

public class StandardTurret : AttackTurret
{
    private void Awake()
    {
        id = 0;
        canAntiAir = true;
        turretParameter = ScriptableObject.CreateInstance<TurretParameter>();
        turretParameter.CopyFrom(Resources.Load<TurretParameter>("StandardTurret/StandardTurretStats" + levels));
        OnLevelIncreased += StandardTurret_OnLevelIncreased;
    }

    private void OnDestroy()
    {
        OnLevelIncreased -= StandardTurret_OnLevelIncreased;
    }

    private void StandardTurret_OnLevelIncreased()
    {
        turretParameter.CopyFrom(Resources.Load<TurretParameter>("StandardTurret/StandardTurretStats" + levels));
        transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("StandardTurret/Material" + levels);
        transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("StandardTurret/Material" + levels);
        transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("StandardTurret/Material" + levels);
    }
}
