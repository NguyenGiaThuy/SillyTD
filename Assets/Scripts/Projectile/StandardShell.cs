using UnityEngine;

public class StandardShell : Projectile
{
    protected override void TargetHit()
    {
        GameObject impactEffect = Instantiate(impactEffectPrefab, transform.position, transform.rotation);

        // Create an explosion
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 10);
        foreach(Collider collider in colliders) 
        {
            // Calculate damage for each armor type
            float finalDamage = sourceTurret.data.damage;
            switch(target.armor) 
            {
                case Mob.ArmorType.Light:
                    finalDamage = finalDamage * 1.5f;
                    break;
                case Mob.ArmorType.Heavy:
                    finalDamage = finalDamage * 0.5f;
                    break;
            }

            collider.GetComponent<Mob>().Hit(Mathf.RoundToInt(finalDamage));
        }
        
        Destroy(impactEffect, 0.5f);
        Destroy(gameObject);   
    }
}
