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
    protected GameObject meshPrefab;
    [SerializeField]
    protected GameObject trailEffectPrefab;
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
        explosionRadius = sourceTurret.stats.explosionRadius;
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
            // Play SFXs and VFXs
            EffectsOnImpacted();
            Destroy(gameObject);
        }

        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    protected void TargetHit()
    {
        // Play SFXs and VFXs
        EffectsOnImpacted();

        // Create an explosion
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, 1 << LayerMask.NameToLayer("Mob"));
        foreach (Collider collider in colliders)
        {
            int finalDamage = GetFinalDamge();
            collider.GetComponent<Mob>().Hit(finalDamage);
        }
        Destroy(gameObject);
    }

    private void EffectsOnImpacted()
    {
        // Explosion
        GameObject impactEffect = Instantiate(impactEffectPrefab, transform.position, transform.rotation);
        Destroy(impactEffect, impactEffect.GetComponent<ParticleSystem>().main.duration);

        // Explosion sound
        AudioSource audioSource = impactEffect.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0.9f;
        if (soundEffects != null) audioSource.PlayOneShot(soundEffects[0]);

        // Trail effect for projectile
        if (trailEffectPrefab != null)
        {
            ParticleSystem trailEffect = trailEffectPrefab.GetComponent<ParticleSystem>();
            trailEffect.transform.parent = null;
            trailEffect.Stop();
            Destroy(trailEffect.gameObject, 5f);
        }
    }

    protected abstract int GetFinalDamge();
}
