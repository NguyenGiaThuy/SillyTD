using UnityEngine;

public class MissileLauncher : AttackTurret
{
    private void Awake()
    {
        ID = 1;
        canAntiAir = true;
        data = Resources.Load<AttackTurretData>("MissileLauncherData/MissileLauncherData" + level);
        LevelIncreased += MissileLauncher_LevelIncreased;
    }

    private void OnDestroy()
    {
        LevelIncreased -= MissileLauncher_LevelIncreased;
    }

    private void MissileLauncher_LevelIncreased()
    {
        data = Resources.Load<AttackTurretData>("MissileLauncherData/MissileLauncherData" + level);
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
            Missile missile = Instantiate(projectilePrefab, firePointPrefab.transform.position, projectilePrefab.transform.rotation).GetComponent<Missile>();
            missile.sourceTurret = this;
            missile.target = target;
            fireCountdown = 0f;
        }

        SetState(TurretState.Rotating);
    }
}
