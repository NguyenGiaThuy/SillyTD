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
            audioSource.Play();

            foreach (GameObject firePointPrefab in firePointPrefabs)
            {
                foreach (GameObject fireEffectPrefab in fireEffectPrefabs) fireEffectPrefab.GetComponent<ParticleSystem>().Play();
                ArtilleryShell artilleryShell = Instantiate(projectilePrefab, firePointPrefab.transform.position, firePointPrefab.transform.rotation).GetComponent<ArtilleryShell>();
                artilleryShell.sourceTurret = this;
                artilleryShell.target = target;
            }

            fireCountdown = 0f;
        }

        SetState(TurretState.Rotating);
    }
}
