using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController), typeof(CharacterStat))]
public class CharacterBase : MonoBehaviour
{
    [Header("CharacterBase")]
    public CharacterController MoveCtrl;
    public CharacterStat Stat;
    public WeaponBase CurrentWeapon;
    public bool IsCover;
    // public float AttackDistance; // Moved to WeaponStat

    protected virtual void Awake()
    {
        MoveCtrl = GetComponent<CharacterController>();
        Stat = GetComponent<CharacterStat>();
    }

    public virtual void OnDamage(HitBoxType hitBoxType)
    {
        Debug.Log($"{gameObject.name} took damage on {hitBoxType} hitbox.");
    }
}
