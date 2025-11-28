using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MovementController), typeof(CharacterStat))]
public class CharacterBase : MonoBehaviour
{
    [Header("CharacterBase")]
    public MovementController MoveCtrl;
    public CharacterStat Stat;
    public Animator Animator;
    [Header("Weapon")]
    public WeaponBase CurrentWeapon;
    public Dictionary<WeaponType,WeaponBase> Weapons;
    public bool IsCover;
    // public float AttackDistance; // Moved to WeaponStat

    protected virtual void Awake()
    {
        MoveCtrl = GetComponent<MovementController>();
        Stat = GetComponent<CharacterStat>();
        InitWeapon();
        
    }

    void InitWeapon()
	{
		Weapons = new ();
        var childWeapons = GetComponentsInChildren<WeaponBase>(true);
        foreach (var weapon in childWeapons)
        {
            weapon.gameObject.SetActive(true);
            Weapons.Add(weapon.GetWeaponType(), weapon);
            weapon.gameObject.SetActive(false);
        }
        CurrentWeapon = null;
	}

    public void EquipWeapon(WeaponType weapon)
	{
        if (CurrentWeapon != null)
            CurrentWeapon.gameObject.SetActive(false);

        if (Weapons.TryGetValue(weapon, out var weaponBase))
        {
            CurrentWeapon = weaponBase;
            CurrentWeapon.gameObject.SetActive(true);
        }
        else CurrentWeapon = null;
	}

    public virtual void OnDamage(HitBoxType hitBoxType)
    {
        Debug.Log($"{gameObject.name} took damage on {hitBoxType} hitbox.");
    }
}
