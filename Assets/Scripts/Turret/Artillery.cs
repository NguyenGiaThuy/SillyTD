using UnityEngine;

public class Artillery : AttackTurret
{
    private void Awake()
    {
        id = 2;
        canAntiAir = false;
        turretParameter = ScriptableObject.CreateInstance<TurretParameter>();
        turretParameter.CopyFrom(Resources.Load<TurretParameter>("Artillery/ArtilleryStats" + levels));
        OnLevelIncreased += Artillery_OnLevelIncreased;
    }

    private void OnDestroy()
    {
        OnLevelIncreased -= Artillery_OnLevelIncreased;
    }

    private void Artillery_OnLevelIncreased()
    {
        turretParameter.CopyFrom(Resources.Load<TurretParameter>("Artillery/ArtilleryStats" + levels));
        transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("Artillery/Material" + levels);
        transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("Artillery/Material" + levels);
        transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("Artillery/Material" + levels);
    }
}
