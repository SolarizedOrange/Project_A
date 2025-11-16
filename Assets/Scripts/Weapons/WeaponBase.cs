using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public WeaponStat Stat;
    protected float lastAttackTime = 0;
    public abstract void Attack(bool hasJustAttacked);
    public abstract WeaponType GetWeaponType();
}
