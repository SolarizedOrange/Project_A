using UnityEngine;

public class SMG: RangedWeapon
{
    public override WeaponType GetWeaponType()
    {
        return WeaponType.SubmachineGun;
    }
}
