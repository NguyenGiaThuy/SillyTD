using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu]
public class AttackTurretData : ScriptableObject
{
    public int damage;
    public float explosionRadius;
    public float fireCooldown;
    public float minRange;
    public float maxRange;
}
