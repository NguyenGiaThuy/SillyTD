using UnityEngine;

[CreateAssetMenu]
public class TurretBlueprint : ScriptableObject
{
    public int id { get { return turretPrefab.GetComponent<Turret>().id; } }
    public TurretParameter[] turretParameters { get; private set; }
    public GameObject turretPrefab { get; private set;  } 
}