using UnityEngine;

public class StandardTurret : AttackTurret
{
    private void Awake()
    {
        ID = 0;
        canAntiAir = true;
        data = Resources.Load<AttackTurretData>("StandardTurretData/StandardTurretData" + level);
        LevelIncreased += StandardTurret_LevelIncreased;
    }

    private void OnDestroy()
    {
        LevelIncreased -= StandardTurret_LevelIncreased;
    }

    private void StandardTurret_LevelIncreased()
    {
        data = Resources.Load<AttackTurretData>("StandardTurretData/StandardTurretData" + level);
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
            foreach (GameObject firePointPrefab in firePointPrefabs)
            {
                foreach (GameObject fireEffectPrefab in fireEffectPrefabs) fireEffectPrefab.GetComponent<ParticleSystem>().Play();
                StandardShell standardShell = Instantiate(projectilePrefab, firePointPrefab.transform.position, projectilePrefab.transform.rotation).GetComponent<StandardShell>();
                standardShell.target = target;
                standardShell.sourceTurret = this;
            }
            fireCountdown = 0f;
        }

        SetState(TurretState.Rotating);
    }
}
