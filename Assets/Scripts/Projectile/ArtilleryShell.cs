using UnityEngine;

public class ArtilleryShell : Projectile
{
    private void Start()
    {
        direction = target.transform.position - transform.position;
        Vector3 force = direction;
        Vector3 offsetForce = force;
        offsetForce.y = transform.position.y - 1f;
        GetComponent<Rigidbody>().AddForce((force.normalized + offsetForce) * speed * Random.Range(0.9f, 1.1f), ForceMode.Impulse);
        explosionRadius = sourceTurret.turretStats.explosionRadius;
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

    protected override int GetFinalDamge()
    {
        // Calculate damage for each armor type
        float finalDamage = sourceTurret.turretStats.damage;
        switch (target.armor)
        {
            case Mob.ArmorType.Light:
                finalDamage = finalDamage * 1.25f;
                break;
            case Mob.ArmorType.Medium:
                finalDamage = finalDamage * 1.25f;
                break;
            case Mob.ArmorType.Heavy:
                finalDamage = finalDamage * 1.25f;
                break;
        }
        return Mathf.RoundToInt(finalDamage);
    }
}
