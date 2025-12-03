using UnityEngine;

public class Shotgun: RangedWeapon
{
    public override void Reload()
    {
        if (Stat.ShotCount.Val < Stat.ShotCount.MaxVal)
        {
            Stat.ShotCount.Val++;
        }
    }

    public override bool CanAttack()
    {
        if (Stat.Capacity.Val > 0 && ((Time.time - lastAttackTime) >= Stat.AttackRate.Val))
        {
            return true;
        }
        return false;
    }

    public override WeaponType GetWeaponType()
    {
        return WeaponType.Shotgun;
    }
}
