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
            audioSource.Play();

            foreach (GameObject firePointPrefab in firePointPrefabs)
            {
                foreach (GameObject fireEffectPrefab in fireEffectPrefabs) fireEffectPrefab.GetComponent<ParticleSystem>().Play();
                Missile missile = Instantiate(projectilePrefab, firePointPrefab.transform.position, firePointPrefab.transform.rotation).GetComponent<Missile>();
                missile.sourceTurret = this;
                missile.target = target;
            }
            fireCountdown = 0f;
        }

        SetState(TurretState.Rotating);
    }
}
