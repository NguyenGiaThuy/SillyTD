using UnityEngine;

public class Artillery : AttackTurret
{
    private void Awake()
    {
        ID = 2;
        canAntiAir = false;
        data = Resources.Load<AttackTurretData>("ArtilleryData/ArtilleryData" + level);
        LevelIncreased += Artillery_LevelIncreased;
    }

    private void OnDestroy()
    {
        LevelIncreased -= Artillery_LevelIncreased;
    }

    private void Artillery_LevelIncreased()
    {
        data = Resources.Load<AttackTurretData>("ArtilleryData/ArtilleryData" + level);
    }

    protected override void Fire()
    {
        if (target == null)
        {
            SetState(TurretState.Idling);
            return;
        }

        // Fire if reloaded
        if (fireCountdown > data.fireCooldown)
        {
            //ParticleSystem particle = Instantiate(fireEffect, firePoint.transform.position, firePoint.transform.rotation, firePoint.transform).GetComponent<ParticleSystem>();
            //Destroy(particle.gameObject, particle.main.duration);
            ArtilleryShell artilleryShell = Instantiate(projectilePrefab, firePointPrefab.transform.position, firePointPrefab.transform.rotation).GetComponent<ArtilleryShell>();
            artilleryShell.sourceTurret = this;
            artilleryShell.target = target;
            fireCountdown = 0f;
        }

        SetState(TurretState.Rotating);
    }
}
