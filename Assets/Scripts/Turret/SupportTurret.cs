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
        id = 3;
        turretParameter = ScriptableObject.CreateInstance<TurretParameter>();
        turretParameter.CopyFrom(Resources.Load<TurretParameter>("SupportTurret/SupportTurretStats" + levels));
        StartCoroutine(BuffNearbyTurrets());
        OnLevelIncreased += SupportTurret_OnLevelIncreased;
    }

    private void OnDestroy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, turretParameter.maxRange, 1 << LayerMask.NameToLayer("BuffedTurret"));
        foreach (Collider collider in colliders)
        {
            AttackTurret turret = collider.GetComponent<AttackTurret>();
            turret.turretParameter.damage = Mathf.RoundToInt(turret.turretParameter.damage / damageMultiplier);
            turret.aura.SetActive(false);
            turret.gameObject.layer = LayerMask.NameToLayer("Turret");
        }
        OnLevelIncreased -= SupportTurret_OnLevelIncreased;
    }

    private void SupportTurret_OnLevelIncreased()
    {
        turretParameter.CopyFrom(Resources.Load<TurretParameter>("SupportTurret/SupportTurretStats" + levels));
        GetComponent<MeshRenderer>().material = Resources.Load<Material>("SupportTurret/Material" + levels);
    }

    private IEnumerator BuffNearbyTurrets()
    {
        // Only search on interval
        while (true)
        {
            
            Collider[] colliders = Physics.OverlapSphere(transform.position, turretParameter.maxRange, 1 << LayerMask.NameToLayer("Turret"));
            foreach (Collider collider in colliders)
            {
                AttackTurret turret = collider.GetComponent<AttackTurret>();
                turret.turretParameter.damage = Mathf.RoundToInt(damageMultiplier * turret.turretParameter.damage);
                turret.aura.SetActive(true);
                turret.gameObject.layer = LayerMask.NameToLayer("BuffedTurret");
            }

            yield return new WaitForSeconds(buffInterval);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, turretParameter.minRange);
        Gizmos.DrawWireSphere(transform.position, turretParameter.maxRange);
    }
}
