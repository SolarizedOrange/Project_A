using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public WeaponStat Stat;
    protected float lastAttackTime = 0;
    public abstract bool Attack(bool hasJustAttacked);
    public abstract WeaponType GetWeaponType();
    public abstract bool CanAttack();
}
