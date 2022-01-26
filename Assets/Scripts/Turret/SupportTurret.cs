using System.Collections;
using UnityEngine;

public class SupportTurret : Turret
{
    [Header("Unity Specifications", order = 0)]
    [Header("Optional", order = 1)]
    [SerializeField]
    private float buffInterval;
    [SerializeField]
    private float damageMultiplier;

    private void Awake()
    {
        ID = 3;
        turretStats = Resources.Load<TurretStats>("SupportTurret/SupportTurretStats" + level);
        StartCoroutine(BuffNearbyTurrets());
        OnLevelIncreased += SupportTurret_OnLevelIncreased;
    }

    private void OnDestroy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, turretStats.maxRange, 1 << LayerMask.NameToLayer("BuffedTurret"));
        foreach (Collider collider in colliders)
        {
            AttackTurret turret = collider.GetComponent<AttackTurret>();
            turret.turretStats.damage = Mathf.RoundToInt(turret.turretStats.damage / damageMultiplier);
            turret.aura.SetActive(false);
            turret.gameObject.layer = LayerMask.NameToLayer("Turret");
        }
        OnLevelIncreased -= SupportTurret_OnLevelIncreased;
    }

    private void SupportTurret_OnLevelIncreased()
    {
        turretStats = Resources.Load<TurretStats>("SupportTurret/SupportTurretStats" + level);
        GetComponent<MeshRenderer>().material = Resources.Load<Material>("SupportTurret/Material" + level);
    }

    private IEnumerator BuffNearbyTurrets()
    {
        // Only search on interval
        while (true)
        {
            
            Collider[] colliders = Physics.OverlapSphere(transform.position, turretStats.maxRange, 1 << LayerMask.NameToLayer("Turret"));
            foreach (Collider collider in colliders)
            {
                AttackTurret turret = collider.GetComponent<AttackTurret>();
                turret.turretStats.damage = Mathf.RoundToInt(damageMultiplier * turret.turretStats.damage);
                turret.aura.SetActive(true);
                turret.gameObject.layer = LayerMask.NameToLayer("BuffedTurret");
            }

            yield return new WaitForSeconds(buffInterval);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, turretStats.minRange);
        Gizmos.DrawWireSphere(transform.position, turretStats.maxRange);
    }
}
