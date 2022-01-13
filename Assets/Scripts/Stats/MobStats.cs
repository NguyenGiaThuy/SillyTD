using System;
using UnityEngine;

[Serializable]
public class MobStats
{
    public float movementSpeed;
    public int killCredits;
    public int killResearchPoints;
    public int healthPoints;
    public Transform[] wayPointsToMove;
    public int nextWayPointIndex;
    public Vector3 nextDirection;
    public Transform transform;
}
