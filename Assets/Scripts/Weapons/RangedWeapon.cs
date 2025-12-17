using System.Collections;
using UnityEngine;

public abstract class RangedWeapon: WeaponBase
{
    public Transform MuzzlePosition;

    [Header("Test Ray Setting")]
    [SerializeField] bool IsFullAuto = true;
    [SerializeField] Color debugRayColor = Color.red;
    public bool IsReloading;
    protected int ammo;
    public int Ammo { get{ return ammo; }}

    void Start()
    {
        ammo = Stat.Capacity;
    }

    void OnDisable()
    {
        IsReloading = false;
    }

    public override bool Attack(bool hasJustAttacked)
    {
        if (CanAttack() && (IsFullAuto || hasJustAttacked))
        {
            Fire();
            IsReloading = false;
            lastAttackTime = Time.time;
            return true;
        }
        return false;
    }
    public void Fire()
    {
        ammo--;
        for (int i = 0; i < Stat.ShotCount; i++)
        {   
            Vector3 rayPos = 0.1f * (1.0f - Stat.Accuracy) * Random.insideUnitSphere;
            rayPos.z *= 0.5f;
            rayPos = (MuzzlePosition.forward + rayPos).normalized;
            Ray ray = new Ray(MuzzlePosition.position, rayPos);
            if (Physics.Raycast(ray, out var hitInfo ,Stat.AttackRange,(int)Layers.HitCollider))
            {
                Debug.DrawRay(MuzzlePosition.position, rayPos * Stat.AttackRange, debugRayColor, 5.0f);
                var res = hitInfo.collider;
                if (res.TryGetComponent<HitBox>(out var hitBox))
                {
                    // var buff = Character.GetWeaponBuffMul(GetWeaponType(), WeaponStatType.Damage);
                    hitBox.OnHit(Stat.Damage);
                }
                else
                {
                    
                }
            }
        }
    }
    public override bool CanAttack()
    {
        if (!IsReloading && ammo > 0 && ((Time.time - lastAttackTime) >= Stat.AttackRate))
        {
            return true;
        }
        return false;
    }
    public virtual void Reload(ObserverInt ammoInventory = null)
    {
        if (!IsReloading)
        {
            if (ammoInventory.Value > 0 && ammo < Stat.Capacity)
            {
                IsReloading = true;
                StartCoroutine(ReloadRoutine(ammoInventory));
            }
        }
    }

    protected virtual IEnumerator ReloadRoutine(ObserverInt ammoInventory)
    {
        yield return new WaitForSeconds(Stat.ReloadTime);
        ammoInventory.Value = ammoInventory.Value + ammo;
        ammo = Mathf.Min(Stat.Capacity, ammoInventory.Value);
        ammoInventory.Value = Mathf.Max(ammoInventory.Value - Stat.Capacity, 0);
        IsReloading = false;
    }
}
