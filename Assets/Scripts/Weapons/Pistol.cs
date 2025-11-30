using UnityEngine;

public class Pistol : RangedWeapon
{
    public void Awake()
    {
        // Stat = new WeaponStat();
        // Stat.AttackRate.Val = 0.2f;
    }
    public override void Attack(bool hasJustAttacked)
    {
        if (hasJustAttacked && ((Time.time - lastAttackTime) >= Stat.AttackRate.Val))
        {
            Fire();
            lastAttackTime = Time.time;
        }
    }

    public override WeaponType GetWeaponType()
    {
        return WeaponType.Handgun;
    }

    public override void Reload()
    {
        if (Stat.Capacity.Val < Stat.Capacity.MaxVal)
        {
            Stat.Capacity.Val = Stat.Capacity.MaxVal;
            Debug.Log("Reload");
        }
    }
}
