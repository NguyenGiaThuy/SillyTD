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
}
