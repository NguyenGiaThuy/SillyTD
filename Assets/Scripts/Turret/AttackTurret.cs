using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackTurret : Turret
{
    public delegate void OnStateChangedHandler(TurretState turretState);
    public event OnStateChangedHandler OnStateChanged;

    public enum TurretState
    {
        Idling,
        Rotating,
        Firing,
    }

    public TurretState CurrentState { get; private set; }

    [Header("Unity Specifications", order = 0)]
    [Header("Optional", order = 1)]
    [SerializeField]
    private float searchInterval;
    [SerializeField]
    private float rotationSpeed;

    [Header("Mandatory", order = 1)]
    [SerializeField]
    private GameObject partToRotatePrefab;
    [SerializeField]
    protected GameObject[] firePointPrefabs;
    [SerializeField]
    protected GameObject[] fireEffectPrefabs;
    [SerializeField]
    protected GameObject projectilePrefab;
    
    // Hidden fields
    protected Mob target;
    protected float fireCountdown;
    protected bool canAntiAir;
    protected AudioSource audioSource;

    private void Start()
    {
        fireCountdown = turretStats.fireRate - 1f;
        SetNewState(TurretState.Idling);
        StartCoroutine(SearchTarget());
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case TurretState.Idling:
                Idle();
                break;
            case TurretState.Rotating:
                Rotate();
                break;
            case TurretState.Firing:
                Fire();
                break;
        }

        // Reload
        if (fireCountdown <= turretStats.fireRate) fireCountdown += Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, turretStats.minRange);
        Gizmos.DrawWireSphere(transform.position, turretStats.maxRange);
    }

    protected void SetNewState(TurretState newTurretState)
    {
        if (newTurretState == CurrentState) return;

        CurrentState = newTurretState;
        OnStateChanged?.Invoke(newTurretState);
    }

    private IEnumerator SearchTarget()
    {
        // Only search on interval
        while (true)
        {
            // If target is out of range, search new target
            if (target == null || Vector3.Distance(transform.position, target.transform.position) > turretStats.maxRange 
                || Vector3.Distance(transform.position, target.transform.position) < turretStats.minRange)
            {
                // Search for ground and flying mobs
                GameObject[] groundMobs = GameObject.FindGameObjectsWithTag("GroundMob");
                GameObject[] flyingMobs = GameObject.FindGameObjectsWithTag("FlyingMob");

                // Combine ground and flying mobs into targets list
                List<Mob> mobs = new List<Mob>();
                foreach (GameObject groundMob in groundMobs) mobs.Add(groundMob.GetComponent<Mob>());
                if (canAntiAir) foreach (GameObject flyingMob in flyingMobs) mobs.Add(flyingMob.GetComponent<Mob>());

                target = GetClosestTargetFromDestination(mobs);
            }

            yield return new WaitForSeconds(searchInterval);
        }
    }

    private Mob GetClosestTargetFromDestination(List<Mob> mobs)
    {
        Mob closestTargetFromEndPoint = null;
        float closestDistanceFromEndPoint = float.PositiveInfinity;

        foreach (var mob in mobs)
        {
            float targetDistance = Vector3.Distance(transform.position, mob.transform.position);
            float targetDistanceFromEndPoint = mob.GetCurrentPathLength();

            if (targetDistance <= turretStats.maxRange 
                && targetDistance >= turretStats.minRange 
                && targetDistanceFromEndPoint < closestDistanceFromEndPoint)
            {
                closestTargetFromEndPoint = mob;
                closestDistanceFromEndPoint = targetDistanceFromEndPoint;
            }
        }

        return closestTargetFromEndPoint;
    }

    private void Idle()
    {
        if (target != null) SetNewState(TurretState.Rotating);
    }

    private void Rotate()
    {
        if (target == null)
        {
            SetNewState(TurretState.Idling);
            return;
        }

        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        //partToRotatePrefab.transform.rotation = Quaternion.LerpUnclamped(partToRotatePrefab.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        partToRotatePrefab.transform.rotation = Quaternion.RotateTowards(partToRotatePrefab.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // Done rotation
        if (Quaternion.Angle(partToRotatePrefab.transform.rotation, Quaternion.LookRotation(direction)) <= 10f) SetNewState(TurretState.Firing);
    }

    private void Fire()
    {
        if (target == null)
        {
            SetNewState(TurretState.Idling);
            return;
        }

        // Fire if reloaded
        if (fireCountdown > turretStats.fireRate)
        {
            audioSource.Play();

            foreach (GameObject firePointPrefab in firePointPrefabs)
            {
                foreach (GameObject fireEffectPrefab in fireEffectPrefabs) fireEffectPrefab.GetComponent<ParticleSystem>().Play();
                Projectile projectile = Instantiate(projectilePrefab, firePointPrefab.transform.position, firePointPrefab.transform.rotation).GetComponent<Projectile>();
                projectile.sourceTurret = this;
                projectile.target = target;
            }

            fireCountdown = 0f;
        }

        SetNewState(TurretState.Rotating);
    }
}
