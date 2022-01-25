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
        damage = turret.attackTurretStats.damage;
        explosionRadius = turret.attackTurretStats.explosionRadius;
        fireCooldown = turret.attackTurretStats.fireCooldown;
        minRange = turret.attackTurretStats.minRange;
        maxRange = turret.attackTurretStats.maxRange;
        level = turret.level;

        Vector3 turretPosition = turret.transform.position;
        position[0] = turretPosition.x;
        position[1] = turretPosition.y;
        position[2] = turretPosition.z;
    }

    public void Load(AttackTurret turret)
    {
        turret.attackTurretStats.damage = damage;
        turret.attackTurretStats.explosionRadius = explosionRadius;
        turret.attackTurretStats.fireCooldown = fireCooldown;
        turret.attackTurretStats.minRange = minRange;
        turret.attackTurretStats.maxRange = maxRange;
        for (int i = 0; i < level - 1; i++) turret.IncreaseLevel();

        Vector3 turretPosition = new Vector3(position[0], position[1], position[2]);
        turret.transform.position = turretPosition;
    }
}
