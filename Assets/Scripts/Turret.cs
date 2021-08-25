using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    [Header("Game Specifications")]
    [SerializeField]
    private float range;
    [SerializeField]
    protected float fireCooldown;
    [SerializeField]
    protected float rotationSpeed;
    [SerializeField]
    protected float elevationSpeed;
    [SerializeField]
    private int cost;
    [SerializeField]
    private int level;

    [Header("Unity Specifications")]
    [SerializeField]
    private float modelHeight;
    [SerializeField]
    private float searchInterval;
    [SerializeField]
    protected GameObject partToRotate;
    [SerializeField]
    protected GameObject partToElevate;
    [SerializeField]
    protected GameObject firePoint;
    [SerializeField]
    protected GameObject fireEffect;

    protected Transform target;
    protected float fireCountdown;

    public int GetCost()
    {
        return cost;
    }

    public int GetLevel()
    {
        return level;
    }

    public float GetModelHeight()
    {
        return modelHeight;
    }

    public void CopyTransform(Turret other)
    {
        transform.position = other.transform.position;
        transform.rotation = other.transform.rotation;
        partToRotate.transform.rotation = other.partToRotate.transform.rotation;
        partToElevate.transform.localRotation = other.partToElevate.transform.localRotation;
    }

    protected IEnumerator SearchTarget()
    {
        // Only search on interval
        while (true)
        {
            // If target is in range, do not change target
            if (!TargetLocked())
            {
                Mob[] mobs = FindObjectsOfType<Mob>();
                target = GetClosestTargetFromDestination(mobs);
            }
            yield return new WaitForSeconds(searchInterval);
        }
    }

    private bool TargetLocked()
    {
        return (target != null && Vector3.Distance(transform.position, target.position) <= range);
    }

    private Transform GetClosestTargetFromDestination(Mob[] mobs)
    {
        Transform closestTargetFromEndPoint = null;
        float closestDistanceFromEndPoint = float.PositiveInfinity;
        foreach (var mob in mobs)
        {
            float targetDistance = Vector3.Distance(transform.position, mob.transform.position);
            float targetDistanceFromEndPoint = mob.GetCurrentPathLength();
            if (targetDistance <= range && targetDistanceFromEndPoint < closestDistanceFromEndPoint)
            {
                closestTargetFromEndPoint = mob.transform;
                closestDistanceFromEndPoint = targetDistanceFromEndPoint;
            }
        }

        return closestTargetFromEndPoint;
    }

    protected abstract void Fire();
    protected abstract void RotateTurret(Vector3 direction);
    protected abstract void ElevateGuns(Vector3 direction);

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    //private Transform GetClosestTarget(Mob[] mobs)
    //{
    //    Transform closestTarget = null;
    //    float closestDistance = float.PositiveInfinity;
    //    foreach (var mob in mobs)
    //    {
    //        float targetDistance = Vector3.Distance(transform.position, mob.transform.position);
    //        if (targetDistance <= range && targetDistance < closestDistance)
    //        {
    //            closestTarget = mob.transform;
    //            closestDistance = targetDistance;
    //        }
    //    }

    //    return closestTarget;
    //}
}
