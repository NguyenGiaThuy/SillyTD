using UnityEngine;

public class ArtilleryTurret : Turret
{
    public TurretState turretState;

    [Header("Unity Specifications")]
    [SerializeField]
    private GameObject bullet;

    private void Awake()
    {
        fireCountdown = fireCooldown - 1f;
    }

    private void Start()
    {
        turretState = TurretState.Idling;
        StartCoroutine(SearchTarget());
    }

    private void Update()
    {
        switch (turretState)
        {
            case TurretState.Idling:
                Idle();
                break;
            case TurretState.Moving:
                Move();
                break;
            case TurretState.Firing:
                Fire();
                break;
        }

        // Reload
        if (fireCountdown <= fireCooldown) {
            fireCountdown += Time.deltaTime;
        }
    }

    protected override void Idle()
    {
        if (target != null)
        {
            turretState = TurretState.Moving;
        } 
    }

    protected override void Move()
    {
        if (target == null)
        {
            turretState = TurretState.Idling;
            return;
        }

        Vector3 direction = target.position - transform.position;
        Rotate(direction);
        Elevate(direction);

        // Done rotation and elevation
        bool isRotated = Quaternion.Angle(partToRotate.transform.rotation, Quaternion.LookRotation(direction)) <= 10f;
        bool isElevated = true;
        if (isRotated && isElevated)
        {
            turretState = TurretState.Firing;
        }
    }

    protected override void Fire()
    {
        if (target == null)
        {
            turretState = TurretState.Idling;
            return;
        }

        // Fire if reloaded
        if (fireCountdown > fireCooldown)
        {
            ParticleSystem particle = Instantiate(fireEffect, firePoint.transform.position, firePoint.transform.rotation, firePoint.transform).GetComponent<ParticleSystem>();
            Destroy(particle.gameObject, particle.main.duration);
            Bullet currentBullet = Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation).GetComponent<Bullet>();
            currentBullet.target = target;
            fireCountdown = 0f;
        }

        turretState = TurretState.Moving;
    }

    private void Rotate(Vector3 direction)
    {
        direction.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        partToRotate.transform.rotation = Quaternion.LerpUnclamped(partToRotate.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    private void Elevate(Vector3 direction)
    {
        // Calculate angle by cosine function
        float targetDistance = direction.magnitude;
        direction.y = 0f;
        float targetRange = direction.magnitude;
        float angle = -Mathf.Acos(targetRange / targetDistance) * 180f / Mathf.PI;
        angle = Mathf.Clamp(angle, -30f, 0f);

        Quaternion lookRotation = Quaternion.Euler(new Vector3(angle, 0f, 0f));
        partToElevate.transform.localRotation = Quaternion.LerpUnclamped(partToElevate.transform.localRotation, lookRotation, elevationSpeed * Time.deltaTime);
    }
}
