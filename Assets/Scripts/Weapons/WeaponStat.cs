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

    public float GetApplyBuffStatBase(WeaponStatType statType, float buffMul)
    {
        StatBase statBase;
		switch (statType)
        {
            case WeaponStatType.Accuracy:
                statBase = stat.Accuracy;
				break;
            case WeaponStatType.AttackRate:
                statBase = stat.AttackRate;
				break;
            case WeaponStatType.Capacity:
                statBase = stat.Capacity;
				break;
            case WeaponStatType.Damage:
                statBase = stat.Damage;
				break;
            case WeaponStatType.ShotCount:
                statBase = stat.ShotCount;
				break;
            case WeaponStatType.AttackRange:
                statBase = stat.AttackRange;
				break;
            case WeaponStatType.Recoil:
                statBase = stat.Recoil;
				break;
            default:
                return 0f;
        }

        if (statBase.IsFloating)
        {
            return Mathf.Clamp(statBase.BaseVal * buffMul, statBase.MinVal, statBase.MaxVal);
        }
        else
        {
            return (int)Mathf.Clamp(statBase.BaseVal * buffMul, statBase.MinVal, statBase.MaxVal);
        }
    }

    public float Accuracy
    {
        get { return GetApplyBuffStatBase(WeaponStatType.Accuracy, owner.GetWeaponBuffMul(type,WeaponStatType.Accuracy)); }
    }

    public float AttackRate
    {
        get { return GetApplyBuffStatBase(WeaponStatType.AttackRate, owner.GetWeaponBuffMul(type,WeaponStatType.AttackRate)); }
    }

    public float Capacity
    {
        get { return GetApplyBuffStatBase(WeaponStatType.Capacity, owner.GetWeaponBuffMul(type,WeaponStatType.Capacity)); }
    }

    public float Damage
    {
        get { return GetApplyBuffStatBase(WeaponStatType.Damage, owner.GetWeaponBuffMul(type,WeaponStatType.Damage)); }
    }

    public float ShotCount
    {
        get { return GetApplyBuffStatBase(WeaponStatType.ShotCount, owner.GetWeaponBuffMul(type,WeaponStatType.ShotCount)); }
    }

    public float AttackRange
    {
        get { return GetApplyBuffStatBase(WeaponStatType.AttackRange, owner.GetWeaponBuffMul(type,WeaponStatType.AttackRange)); }
    }

    public float Recoil
    {
        get { return GetApplyBuffStatBase(WeaponStatType.Recoil, owner.GetWeaponBuffMul(type,WeaponStatType.Recoil)); }
    }
}
