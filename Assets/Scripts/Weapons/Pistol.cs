using UnityEngine;

public class Pistol : RangedWeapon
{
    public override WeaponType GetWeaponType()
    {
        return WeaponType.Handgun;
    }
}
