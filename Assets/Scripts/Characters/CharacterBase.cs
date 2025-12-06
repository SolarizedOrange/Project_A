using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[RequireComponent(typeof(MovementController))]
public class CharacterBase : MonoBehaviour
{
    [Header("CharacterBase")]
    public MovementController MoveCtrl;
    public Animator Animator;
    public bool IsCover;

    [Header("CharacterStat")]
    public CharacterStat Stat;
    public int HP { get; set; }

#region Buff
    [Header("BuffManager")]
    public BuffManager<CharacterStatType> BuffManager;
    public List<CharacterStatPerkItem> PerkList;

    // public CharacterStatPerkItem TestPerk;
    public void AddPerk(ItemBase perk)
	{ 
		if (perk is CharacterStatPerkItem characterPerk)
		{
            foreach (var buff in characterPerk.BuffList)
			{
			    BuffManager.AddBuff(buff);
                PerkList.Add(characterPerk);
			}
		}
        else if (perk is WeaponStatPerkItem weaponPerk)
		{
            foreach (var container in weaponPerk.BuffList)
            {
                if (Weapons.TryGetValue(container.WeaponType,out var weapon))
				{
					weapon.BuffManager.AddBuff(container.Buff);
                    weapon.PerkList.Add(weaponPerk);
				}
            }
		}
	}
#endregion

#region Weapon
    [Header("Weapon")]
    public WeaponBase CurrentWeapon;
    public Dictionary<WeaponType,WeaponBase> Weapons;
    // public float AttackDistance; // Moved to WeaponStat
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
#endregion

    protected virtual void Awake()
    {
        BuffManager = new ();
        PerkList = new ();
        MoveCtrl = GetComponent<MovementController>();
        InitWeapon();
    }

	public virtual void OnDamage(HitBoxType hitBoxType)
    {
        Debug.Log($"{gameObject.name} took damage on {hitBoxType} hitbox.");
    }
}
