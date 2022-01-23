using System;
using UnityEngine;

[CreateAssetMenu]
public class TurretBlueprint : ScriptableObject
{
    [Serializable]
    public class Model
    {
        public int id;
        public int buildCost;
        public int[] upgradeCosts;
        public GameObject turretPrefab;
    }

    public Model[] models;
}


