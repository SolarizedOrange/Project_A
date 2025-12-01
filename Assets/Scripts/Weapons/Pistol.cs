using UnityEngine;

public class Pistol : RangedWeapon
{
    public override bool Attack(bool hasJustAttacked)
    {
        //Implement Semiauto
        if (hasJustAttacked)
        {
            return base.Attack(hasJustAttacked);
        }
        return false;
    }
    public override WeaponType GetWeaponType()
    {
        return WeaponType.Handgun;
    }
}
