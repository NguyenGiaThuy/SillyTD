using System;
using UnityEngine;

[Serializable]
public struct SupportTurretData
{
    public int id;
    public int levels;
    public float[] position;

    public void Save(SupportTurret turret)
    {
        id = turret.id;
        levels = turret.levels;

        Vector3 turretPosition = turret.transform.position;
        position = new float[3];
        position[0] = turretPosition.x;
        position[1] = turretPosition.y;
        position[2] = turretPosition.z;
    }
}
