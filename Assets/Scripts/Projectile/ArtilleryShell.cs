using UnityEngine;

public class ArtilleryShell : Projectile
{
    private void Start()
    {
        direction = target.transform.position - transform.position;
        Vector3 force = direction;
        force.y = transform.position.y;
        GetComponent<Rigidbody>().AddForce(force * speed * Random.Range(0.9f, 1.1f), ForceMode.Impulse);
        explosionRadius = sourceTurret.data.explosionRadius;
        Destroy(gameObject, 5f);
    }

    private void Update() 
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        partToRotatePrefab.transform.rotation = Quaternion.LerpUnclamped(partToRotatePrefab.transform.rotation, lookRotation, 10f * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TargetHit();
    }

    protected override void TargetHit()
    {
        GameObject impactEffect = Instantiate(impactEffectPrefab, transform.position, transform.rotation);
        AudioSource audioSource = impactEffect.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0.9f;
        audioSource.PlayOneShot(soundEffects[0]);

        // Create an explosion
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, 1 << LayerMask.NameToLayer("Mob"));
        foreach (Collider collider in colliders)
        {
            // Calculate damage for each armor type
            float finalDamage = sourceTurret.data.damage;
            //switch (target.armor)
            //{
            //    case Mob.ArmorType.Light:
            //        finalDamage = finalDamage * 0.75f;
            //        break;
            //    case Mob.ArmorType.Heavy:
            //        finalDamage = finalDamage * 1.5f;
            //        break;
            //}

            collider.GetComponent<Mob>().Hit(Mathf.RoundToInt(finalDamage));
        }

        Destroy(impactEffect, impactEffect.GetComponent<ParticleSystem>().main.duration);
        ParticleSystem trailEffect = trailEffectPrefab.GetComponent<ParticleSystem>();
        trailEffect.transform.parent = null;
        trailEffect.Stop();
        Destroy(trailEffect.gameObject, 5f);
        Destroy(gameObject);
    }
}
