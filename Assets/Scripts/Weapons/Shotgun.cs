using System.Collections;
using UnityEngine;

public class Shotgun: RangedWeapon
{

    bool temp;
    public override void Reload()
    {
        if (ammo < Stat.Capacity)
        {
            if (!temp)
            {
                temp = true;
                StartCoroutine(ReloadRoutine());            
            }
            IsReloading = true;
        }
        else
        {
            IsReloading = false;        
        }
    }

    public override bool CanAttack()
    {
        if (ammo > 0 && ((Time.time - lastAttackTime) >= Stat.AttackRate))
        {
            return true;
        }
        return false;
    }
    protected override IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(1f);
        if (IsReloading)
        {
            ammo++;
        }
        temp = false;
    }

    public override WeaponType GetWeaponType()
    {
        return WeaponType.Shotgun;
    }
}
