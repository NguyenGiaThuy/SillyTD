using UnityEngine;

[CreateAssetMenu]
public class TurretBlueprint : ScriptableObject
{
    public int id;
    public int buildCost;
    public int[] upgradeCosts;
    public GameObject turretPrefab;
}
