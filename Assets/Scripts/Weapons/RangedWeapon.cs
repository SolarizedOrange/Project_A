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

    void Start()
    {
        ammo = Stat.Capacity.BaseVal;
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
        Debug.Log("FIRE");
        ammo--;
        for (int i = 0; i < Stat.ShotCount.BaseVal; i++)
        {   
            Vector3 rayPos = 0.1f * (1.0f - Stat.Accuracy.BaseVal) * Random.insideUnitSphere;
            rayPos.z *= 0.5f;
            rayPos = (MuzzlePosition.forward + rayPos).normalized;
            Ray ray = new Ray(MuzzlePosition.position, rayPos);
            if (Physics.Raycast(ray, out var hitInfo ,Stat.AttackRange.BaseVal,(int)Layers.HitCollider))
            {
                Debug.DrawRay(MuzzlePosition.position, rayPos * Stat.AttackRange.BaseVal, debugRayColor, 5.0f);
                Debug.Log("RayCastHit");
                var res = hitInfo.collider;
                if (res.TryGetComponent<HitBox>(out var hitBox))
                {
                    hitBox.OnHit();
                }
                else
                {
                    
                }
            }
        }
    }
    public override bool CanAttack()
    {
        if (!IsReloading && ammo > 0 && ((Time.time - lastAttackTime) >= Stat.AttackRate.BaseVal))
        {
            return true;
        }
        return false;
    }
    public virtual void Reload()
    {
        if (ammo < Stat.Capacity.BaseVal && !IsReloading)
        {
            IsReloading = true;
            StartCoroutine(ReloadRoutine());
        }
    }

    protected virtual IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(3f);
        ammo = Stat.Capacity.BaseVal;
        IsReloading = false;
    }
}
