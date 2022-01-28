using UnityEngine;

[CreateAssetMenu]
public class TurretParameter : ScriptableObject
{
    public int cost;
    public int damage;
    public float explosionRadius;
    public float fireRate;
    public float minRange;
    public float maxRange;

    public void LoadFrom(TurretParameter other)
    {
        cost = other.cost;
        damage = other.damage;
        explosionRadius = other.explosionRadius;
        fireRate = other.fireRate;
        minRange = other.minRange;
        maxRange = other.maxRange;
    }              
}
