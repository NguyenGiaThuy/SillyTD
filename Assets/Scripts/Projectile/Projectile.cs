using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [Header("Game Specifications", order = 0)]
    [SerializeField]
    protected float speed;

    [Header("Unity Specifications", order = 0)]
    [Header("Optional", order = 1)]
    public AttackTurret sourceTurret;
    public Mob target;

    [Header("Mandatory", order = 1)]
    [SerializeField]
    protected GameObject partToRotatePrefab;
    [SerializeField]
    protected GameObject impactEffectPrefab;
    [SerializeField]
    protected AudioClip[] soundEffects;

    // Hidden fields
    protected Vector3 direction;
    protected float explosionRadius;
    
    private void Start() 
    {
        explosionRadius = sourceTurret.data.explosionRadius;
    }

    private void Update()
    {
        if (target != null)
        {
            direction = target.transform.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;
            transform.LookAt(target.transform);

            //Prevent flying over target
            if (direction.magnitude <= distanceThisFrame)
            {
                TargetHit();
                return;
            }
        }
        else
        {
            Destroy(gameObject);
            GameObject impactEffect = Instantiate(impactEffectPrefab, transform.position, transform.rotation);
            Destroy(impactEffect, 0.5f);
        }

        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    protected abstract void TargetHit();
}
