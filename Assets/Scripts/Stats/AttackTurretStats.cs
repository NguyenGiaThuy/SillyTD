using System;
using UnityEngine;

[Serializable]
public class AttackTurretStats
{
    public int id;
    public int damage;
    public float explosionRadius;
    public float fireCooldown; 
    public float minRange;
    public float maxRange;
    public int level;
    public Transform transform;
}
