using UnityEngine;

public class Missile : Projectile
{
    protected override int GetFinalDamge()
    {
        // Calculate damage for each armor type
        float finalDamage = sourceTurret.attackTurretStats.damage;
        switch(target.armor) 
        {
            case Mob.ArmorType.Light:
                finalDamage = finalDamage * 0.75f;
                break;
        case Mob.ArmorType.Medium:
            finalDamage = finalDamage * 0.75f;
            break;
        case Mob.ArmorType.Heavy:
                finalDamage = finalDamage * 1.5f;
                break;
        }
        return Mathf.RoundToInt(finalDamage);
    }
}
