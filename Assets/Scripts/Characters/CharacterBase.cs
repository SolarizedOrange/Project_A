using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Assertions;

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

    public void EquipWeapon(WeaponBase weapon)
	{
        if (CurrentWeapon != null)
            CurrentWeapon.gameObject.SetActive(false);

        CurrentWeapon = weapon;

        if (CurrentWeapon != null)
		{
            CurrentWeapon.gameObject.SetActive(true);
            CurrentWeapon.InitWeapon(this);
			CurrentWeapon.ParentConstraint.SetSource(0, handConstraint);
		}
	}
#endregion

#region Ragdoll
    Collider[] ragdollColliders;
    Rigidbody[] ragdollRigidbodies;

    void InitRagdoll()
    {
        ragdollColliders = Animator.gameObject.GetComponentsInChildren<Collider>();
        ragdollRigidbodies = Animator.gameObject.GetComponentsInChildren<Rigidbody>();

        Assert.AreEqual(ragdollColliders.Length, ragdollRigidbodies.Length);
    }

    void ActivateRagdoll()
    {
        Rigidbody ctrlRigidbody = GetComponent<Rigidbody>();
        Animator.enabled = false;
        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            ragdollColliders[i].enabled = true;
            ragdollRigidbodies[i].isKinematic = false;
            ragdollRigidbodies[i].linearVelocity = ctrlRigidbody.linearVelocity;
            ragdollRigidbodies[i].angularVelocity = ctrlRigidbody.angularVelocity;
        }

        MoveCtrl.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;

        if (CurrentWeapon != null)
        {
            CurrentWeapon.ActivatePhysics();
        }
    }

    void DeactivateRagdoll()
    {
        Animator.enabled = true;
        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            ragdollColliders[i].enabled = false;
            ragdollRigidbodies[i].isKinematic = true;
        }

        MoveCtrl.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().enabled = true;

        if (CurrentWeapon != null)
        {
            CurrentWeapon.DeactivatePhysics();
        }
    }
#endregion

    protected virtual void Awake()
    {
        MoveCtrl = GetComponent<MovementController>();
        InitPerk();
        InitStat();
        InitRagdoll();
        handConstraint = new ();
        handConstraint.weight = 1f;
        handConstraint.sourceTransform = handPosition;

        DeactivateRagdoll();
    }

    protected virtual void Start()
    {
        EquipWeapon(CurrentWeapon);
    }

    public void Die()
    {
        foreach (var col in GetComponentsInChildren<HitBox>())
        {
            col.GetComponent<Collider>().enabled = false;
        }
        ActivateRagdoll();
    }

#if DEBUG
    public void Revive()
    {
        DeactivateRagdoll();
    }
#endif

	public virtual void OnDamage(HitBoxType hitBoxType, float damage) {}
}
