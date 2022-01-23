using System;
using UnityEngine;

[Serializable]
public class AttackTurretData
{
    public int id;
    public int damage;
    public float explosionRadius;
    public float fireCooldown; 
    public float minRange;
    public float maxRange;
    public int level;
    public float[] position;
    public float[] quarternion;

    public AttackTurretData()
    {
        position = new float[3];
        quarternion = new float[4];
    }

    public void Save(AttackTurret turret)
    {
        id = turret.ID;
        damage = turret.stats.damage;
        explosionRadius = turret.stats.explosionRadius;
        fireCooldown = turret.stats.fireCooldown;
        minRange = turret.stats.minRange;
        maxRange = turret.stats.maxRange;
        level = turret.level;

        Vector3 turretPosition = turret.transform.position;
        position[0] = turretPosition.x;
        position[1] = turretPosition.y;
        position[2] = turretPosition.z;
    }

    public void Load(AttackTurret turret)
    {
        turret.stats.damage = damage;
        turret.stats.explosionRadius = explosionRadius;
        turret.stats.fireCooldown = fireCooldown;
        turret.stats.minRange = minRange;
        turret.stats.maxRange = maxRange;
        turret.level = level;

        Vector3 turretPosition = new Vector3(position[0], position[1], position[2]);
        turret.transform.position = turretPosition;
    }
}
