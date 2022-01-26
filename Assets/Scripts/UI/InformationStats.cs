using UnityEngine;

[CreateAssetMenu]
public class InformationStats : ScriptableObject
{
    public Sprite avatar;
    public string description;
    public TurretBlueprint blueprint;
    public TurretStats[] turretStats;
}
