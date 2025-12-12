using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(ParentConstraint))]
public abstract class WeaponBase : MonoBehaviour
{
    [Header("WeaponBase Settings")]
    [SerializeField] WeaponStat stat;
    public WeaponStatWrapper Stat;
    public ParentConstraint ParentConstraint;
    protected float lastAttackTime = 0;

    protected virtual void Awake()
	{
		ParentConstraint = GetComponent<ParentConstraint>();
	}

    public void InitWeapon(CharacterBase character)
	{
        Stat = new(character,GetWeaponType(),stat);
	}

	public abstract bool Attack(bool hasJustAttacked);
    public abstract WeaponType GetWeaponType();
    public abstract bool CanAttack();
}
