using System.Collections;
using UnityEngine;

public abstract class AttackTurret : Turret
{
    public delegate void OnStateChangedHandler(TurretState newGameState);
    public event OnStateChangedHandler StateChanged;

    public enum TurretState
    {
        Null,
        Idling,
        Rotating,
        Firing,
    }

    public TurretState State { get; private set; }

    //[Header("Game Specifications", order = 0)]
    //[Header("Optional", order = 1)]
    //public int level;
    [Header("Mandatory", order = 1)]
    public AttackTurretStats stats;

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
        fireCountdown = stats.fireCooldown - 1f;
        SetState(TurretState.Idling);
        StartCoroutine(SearchTarget());
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        switch (State)
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
        if (fireCountdown <= stats.fireCooldown) fireCountdown += Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, stats.minRange);
        Gizmos.DrawWireSphere(transform.position, stats.maxRange);
    }

    protected void SetState(TurretState turretState)
    {
        if (turretState == State) return;

        State = turretState;
        StateChanged?.Invoke(turretState);
    }

    private IEnumerator SearchTarget()
    {
        // Only search on interval
        while (true)
        {
            // If target is out of range, search new target
            if (target == null || Vector3.Distance(transform.position, target.transform.position) > stats.maxRange 
                || Vector3.Distance(transform.position, target.transform.position) < stats.minRange)
            {
                // Search for ground and flying mobs
                GameObject[] groundMobs = GameObject.FindGameObjectsWithTag("GroundMob");
                GameObject[] flyingMobs = GameObject.FindGameObjectsWithTag("FlyingMob");

                // Combine ground and flying mobs into targets list
                Mob[] mobs = new Mob[groundMobs.Length + flyingMobs.Length];
                int i = 0;
                foreach (GameObject groundMob in groundMobs) mobs[i++] = groundMob.GetComponent<Mob>();
                if (canAntiAir) foreach (GameObject flyingMob in flyingMobs) mobs[i++] = flyingMob.GetComponent<Mob>();

                target = GetClosestTargetFromDestination(mobs);
            }

            yield return new WaitForSeconds(searchInterval);
        }
    }

    private Mob GetClosestTargetFromDestination(Mob[] mobs)
    {
        Mob closestTargetFromEndPoint = null;
        float closestDistanceFromEndPoint = float.PositiveInfinity;

        foreach (var mob in mobs)
        {
            float targetDistance = Vector3.Distance(transform.position, mob.transform.position);
            float targetDistanceFromEndPoint = mob.GetCurrentPathLength();

            if (targetDistance <= stats.maxRange 
                && targetDistance >= stats.minRange 
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
        if (target != null) SetState(TurretState.Rotating);
    }

    private void Rotate()
    {
        if (target == null)
        {
            SetState(TurretState.Idling);
            return;
        }

        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        //partToRotatePrefab.transform.rotation = Quaternion.LerpUnclamped(partToRotatePrefab.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        partToRotatePrefab.transform.rotation = Quaternion.RotateTowards(partToRotatePrefab.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // Done rotation
        if (Quaternion.Angle(partToRotatePrefab.transform.rotation, Quaternion.LookRotation(direction)) <= 10f) SetState(TurretState.Firing);
    }

    private void Fire()
    {
        if (target == null)
        {
            SetState(TurretState.Idling);
            return;
        }

        // Fire if reloaded
        if (fireCountdown > stats.fireCooldown)
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

        SetState(TurretState.Rotating);
    }
}
