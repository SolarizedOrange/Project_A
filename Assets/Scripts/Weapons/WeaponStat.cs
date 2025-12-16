using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStat", menuName = "Weapon/WeaponStat")]
public class WeaponStat : ScriptableObject
{
	public StatBase Accuracy;
	public StatBase AttackRate;
	public StatBase Capacity;
	public StatBase Damage;
	public StatBase ShotCount;
	public StatBase AttackRange;
	public StatBase Recoil;
    public StatBase ReloadTime;
}

public class WeaponStatWrapper
{
	WeaponType type;
    WeaponStat stat;
    CharacterBase owner;

    public WeaponStatWrapper(CharacterBase owner, WeaponType type , WeaponStat stat)
    {
        this.owner = owner;
        this.stat = stat;
		this.type = type;
    }

    public float Accuracy
    {
        get { return Mathf.Clamp(stat.Accuracy.BaseVal * owner.GetWeaponBuffMul(type,WeaponStatType.Accuracy), stat.Accuracy.MinVal,stat.Accuracy.MaxVal ); }
    }

    public float AttackRate
    {
        get { return Mathf.Clamp(stat.AttackRate.BaseVal * owner.GetWeaponBuffMul(type,WeaponStatType.AttackRate), stat.AttackRate.MinVal,stat.AttackRate.MaxVal ); }
    }

    public int Capacity
    {
        get { return (int)Mathf.Clamp(stat.Capacity.BaseVal * owner.GetWeaponBuffMul(type,WeaponStatType.Capacity), stat.Capacity.MinVal,stat.Capacity.MaxVal ); }
    }

    public float Damage
    {
        get { return Mathf.Clamp(stat.Damage.BaseVal * owner.GetWeaponBuffMul(type,WeaponStatType.Damage), stat.Damage.MinVal,stat.Damage.MaxVal ); }
    }

    public int ShotCount
    {
        get { return (int)Mathf.Clamp(stat.ShotCount.BaseVal * owner.GetWeaponBuffMul(type,WeaponStatType.ShotCount), stat.ShotCount.MinVal,stat.ShotCount.MaxVal ); }
    }

    public float AttackRange
    {
        get { return Mathf.Clamp(stat.AttackRange.BaseVal * owner.GetWeaponBuffMul(type,WeaponStatType.AttackRange), stat.AttackRange.MinVal,stat.AttackRange.MaxVal ); }
    }

    public float Recoil
    {
        get { return Mathf.Clamp(stat.Recoil.BaseVal * owner.GetWeaponBuffMul(type,WeaponStatType.Recoil), stat.Recoil.MinVal,stat.Recoil.MaxVal ); }
    }

    public float ReloadTime
    {
        get { return Mathf.Clamp(stat.ReloadTime.BaseVal * owner.GetWeaponBuffMul(type,WeaponStatType.ReloadTime), stat.ReloadTime.MinVal,stat.ReloadTime.MaxVal ); }
    }
}
