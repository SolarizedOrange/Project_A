using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(ParentConstraint))]
public abstract class WeaponBase : MonoBehaviour
{
    [Header("WeaponBase Settings")]
    [SerializeField] WeaponStat stat;
    public WeaponStatWrapper Stat;
    [HideInInspector] public ParentConstraint ParentConstraint;
    [HideInInspector] public Rigidbody Rigidbody;
    [HideInInspector] public Collider Collider;
    protected float lastAttackTime = 0;

    protected virtual void Awake()
	{
		ParentConstraint = GetComponent<ParentConstraint>();
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        DeactivatePhysics();
	}

    public virtual void InitWeapon(CharacterBase character)
	{
        Stat = new(character,GetWeaponType(),stat);
	}

	public abstract bool Attack(bool hasJustAttacked);
    public abstract WeaponType GetWeaponType();
    public abstract bool CanAttack();
    public void ActivatePhysics()
    {
        Rigidbody.isKinematic = false;
        Collider.enabled = true;

        ParentConstraint.enabled = false;
    }
    public void DeactivatePhysics()
    {
        Rigidbody.isKinematic = true;
        Collider.enabled = false;
        ParentConstraint.enabled = true;
    }
}
