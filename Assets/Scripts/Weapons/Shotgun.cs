using System.Collections;
using UnityEngine;

public class Shotgun: RangedWeapon
{
    public override void Reload(ObserverInt ammoInventory)
    {
        if (!IsReloading)
        {
            IsReloading = true;
            StartCoroutine(ReloadRoutine(ammoInventory));
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
    protected override IEnumerator ReloadRoutine(ObserverInt ammoInventory)
    {
        while (IsReloading && ammo < Stat.Capacity && ammoInventory.Value > 0)
        {
            yield return new WaitForSeconds(Stat.ReloadTime);
            if (IsReloading)
            {
                ammoInventory.Value--;
                ammo++;
            }
        }
        IsReloading = false;
    }

    public override WeaponType GetWeaponType()
    {
        return WeaponType.Shotgun;
    }
}
