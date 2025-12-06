using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuffManager<WeaponStatType>))]
public abstract class WeaponBase : MonoBehaviour
{
    [Header("WeaponBase Settings")]
    public WeaponStat Stat;
    protected float lastAttackTime = 0;

    [Header("BuffManager")]
    public BuffManager<WeaponStatType> BuffManager;
    public List<WeaponStatPerkItem> PerkList;

	protected virtual void Awake()
	{
		BuffManager = new();
        PerkList = new();
	}

	public abstract bool Attack(bool hasJustAttacked);
    public abstract WeaponType GetWeaponType();
    public abstract bool CanAttack();
}
