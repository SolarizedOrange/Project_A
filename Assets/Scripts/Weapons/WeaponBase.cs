using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(ParentConstraint))]
public abstract class WeaponBase : MonoBehaviour
{
    [Header("WeaponBase Settings")]
    public WeaponStat Stat;
    public ParentConstraint ParentConstraint;
    protected float lastAttackTime = 0;

    protected virtual void Awake()
	{
		this.ParentConstraint = GetComponent<ParentConstraint>();
	}

	public abstract bool Attack(bool hasJustAttacked);
    public abstract WeaponType GetWeaponType();
    public abstract bool CanAttack();
}
