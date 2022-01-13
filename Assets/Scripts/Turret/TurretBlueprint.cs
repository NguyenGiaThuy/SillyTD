using System;
using UnityEngine;

[Serializable]
public class TurretBlueprint
{
    [Serializable]
    public class Model
    {
        public int id;
        public int cost;
        public GameObject turretPrefab;
    }

    public Model[] models;
}


