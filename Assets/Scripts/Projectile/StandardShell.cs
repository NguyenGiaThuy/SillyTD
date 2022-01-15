using UnityEngine;

public class StandardShell : Projectile
{
    protected override int GetFinalDamge()
    {
        // Calculate damage for each armor type
        float finalDamage = sourceTurret.data.damage;
        switch (target.armor)
        {
            case Mob.ArmorType.Light:
                finalDamage = finalDamage * 1.5f;
                break;
            case Mob.ArmorType.Medium:
                finalDamage = finalDamage * 1f;
                break;
            case Mob.ArmorType.Heavy:
                finalDamage = finalDamage * 0.5f;
                break;
        }
        return Mathf.RoundToInt(finalDamage);
    }
}
