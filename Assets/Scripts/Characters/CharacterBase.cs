using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Animations;

[RequireComponent(typeof(MovementController))]
public class CharacterBase : MonoBehaviour
{
    [Header("CharacterBase")]
    public MovementController MoveCtrl;
    public Animator Animator;
    public bool IsCover;

    [Header("CharacterStat")]
    [SerializeField] CharacterStat stat; 
    public CharacterStatWrapper Stat ;
    public ObserverFloat HP;
    public ObserverInt Money;
    public int XP { get; set; }
    public Dictionary<WeaponType,ObserverInt> BulletAmmo;
    public ObserverBool HasArmor;

    void InitStat()
	{
        Stat = new (this, stat);
        HP = new () { Value = Stat.Hp };
        Money = new (Money);
        HasArmor = new (HasArmor);
        // Bullet Init
        BulletAmmo = new ();
        for (int i = 0; i < (int)WeaponType.Length; i++)
		{
			BulletAmmo.Add((WeaponType)i, new ());
		}
	}
#region Perk
    [Header("Perk")]
    public BuffManager CharacterBuff;
    public Dictionary<WeaponType,BuffManager> WeaponBuff;
    public Dictionary<PerkGroup,PerkItem> PerkDic;

    void InitPerk()
	{
		CharacterBuff = new();
        WeaponBuff = new();
        PerkDic = new ();
	}

    // public CharacterStatPerkItem TestPerk;
    public void AddPerk(PerkItem perk)
	{ 
        RemovePerk(perk.PerkGroup);
        foreach (var buffContainer in perk.BuffList)
        {
            var buff = buffContainer.Buff;
            if (buff is CharacterBuff)
			{
				CharacterBuff.AddBuff(buff);
			}
            else if (buff is WeaponBuff weaponBuff)
			{
                var weaponType = weaponBuff.WeaponType;
				if (WeaponBuff.ContainsKey(weaponType)==false)
                    WeaponBuff.Add(weaponType, new ());
                WeaponBuff[weaponType].AddBuff(buff);
			}
        }
        PerkDic.Add(perk.PerkGroup, perk);
	}

    public void RemovePerk(PerkGroup group)
	{
        if (PerkDic.TryGetValue(group,out var lastPerk))
        {
            foreach (var buffContainer in lastPerk.BuffList)
            {
                var buff = buffContainer.Buff;
                if (buff is CharacterBuff)
                {
                    CharacterBuff.RemoveBuff(buff);
                }
                else if (buff is WeaponBuff weaponBuff)
                {
                    var weaponType = weaponBuff.WeaponType;
                    if (WeaponBuff.ContainsKey(weaponType))
                        WeaponBuff[weaponType].RemoveBuff(buff);
                }
            }
        }
        PerkDic.Remove(group);
        return;
    }

    public float GetWeaponBuffMul(WeaponType weaponType, WeaponStatType statType)
	{
		if (WeaponBuff.TryGetValue(weaponType, out var buffManager) == false) return 1f;
        return buffManager.GetBuffMul(statType);
	}

#endregion

#region Weapon
    [Header("Weapon")]
    [SerializeField] Transform handPosition;
    public WeaponBase CurrentWeapon;
    public List<InventoryWeaponSlot> Weapons;
    ConstraintSource handConstraint;
    // public float AttackDistance; // Moved to WeaponStat
    void InitWeapon()
	{
		Weapons = UIController.Instance.Inventory.WeaponSlots;
        handConstraint = new ();
        handConstraint.weight = 1f;
        handConstraint.sourceTransform = handPosition;
	}

    public void EquipWeapon(WeaponBase weapon)
	{
        if (CurrentWeapon != null)
            CurrentWeapon.gameObject.SetActive(false);

        CurrentWeapon = weapon;

        if (CurrentWeapon != null)
		{
            CurrentWeapon.InitWeapon(this);
			CurrentWeapon.ParentConstraint.SetSource(0,handConstraint);
            CurrentWeapon.gameObject.SetActive(true);
		}
	}
#endregion

    protected virtual void Awake()
    {
        MoveCtrl = GetComponent<MovementController>();
        InitPerk();
        InitStat();
        if (CurrentWeapon != null) CurrentWeapon.InitWeapon(this);
    }

	void Start()
	{
        InitWeapon();
	}

	public virtual void OnDamage(HitBoxType hitBoxType, float damage)
    {
        Debug.Log($"{gameObject.name} took damage on {hitBoxType} hitbox.");
        HP.Value -= damage;
    }
}
