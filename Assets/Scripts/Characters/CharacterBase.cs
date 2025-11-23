using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MovementController), typeof(CharacterStat))]
public class CharacterBase : MonoBehaviour
{
    [Header("CharacterBase")]
    public MovementController MoveCtrl;
    public CharacterStat Stat;
    public WeaponBase CurrentWeapon;
    public Animator Animator;
    public bool IsCover;
    // public float AttackDistance; // Moved to WeaponStat

    protected virtual void Awake()
    {
        MoveCtrl = GetComponent<MovementController>();
        Stat = GetComponent<CharacterStat>();
    }

    public virtual void OnDamage(HitBoxType hitBoxType)
    {
        Debug.Log($"{gameObject.name} took damage on {hitBoxType} hitbox.");
    }
}
