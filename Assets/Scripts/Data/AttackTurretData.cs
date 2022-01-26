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

    public AttackTurretData()
    {
        position = new float[3]; 
    }

    public void Save(AttackTurret turret)
    {
        id = turret.ID;
        damage = turret.turretStats.damage;
        explosionRadius = turret.turretStats.explosionRadius;
        fireCooldown = turret.turretStats.fireRate;
        minRange = turret.turretStats.minRange;
        maxRange = turret.turretStats.maxRange;
        level = turret.level;

        Vector3 turretPosition = turret.transform.position;
        position[0] = turretPosition.x;
        position[1] = turretPosition.y;
        position[2] = turretPosition.z;
    }

    public void Load(AttackTurret turret)
    {
        turret.turretStats.damage = damage;
        turret.turretStats.explosionRadius = explosionRadius;
        turret.turretStats.fireRate = fireCooldown;
        turret.turretStats.minRange = minRange;
        turret.turretStats.maxRange = maxRange;
        for (int i = 0; i < level - 1; i++) turret.IncreaseLevel();

        Vector3 turretPosition = new Vector3(position[0], position[1], position[2]);
        turret.transform.position = turretPosition;
    }
}
