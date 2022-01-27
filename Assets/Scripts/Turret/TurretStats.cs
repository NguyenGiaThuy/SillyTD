using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu]
public class TurretStats : ScriptableObject
{
    public int damage;
    public float explosionRadius;
    public float fireRate;
    public float minRange;
    public float maxRange;

    public void CopyFrom(TurretStats other)
    {
        damage = other.damage;
        explosionRadius = other.explosionRadius;
        fireRate = other.fireRate;
        minRange = other.minRange;
        maxRange = other.maxRange;
    }              
}
