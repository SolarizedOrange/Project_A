using System.Collections;
using UnityEngine;

public abstract class RangedWeapon: WeaponBase
{
    public Transform MuzzlePosition;

    [Header("Test Ray Setting")]
    [SerializeField] bool IsFullAuto = true;
    [SerializeField] Color debugRayColor = Color.red;
    public bool IsReloading;

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
        Stat.Capacity.Val--;
        for (int i = 0; i < Stat.ShotCount.Val; i++)
        {   
            Vector3 rayPos = 0.1f * (1.0f - Stat.Accuracy.Val) * Random.insideUnitSphere;
            rayPos.z *= 0.5f;
            rayPos = (MuzzlePosition.forward + rayPos).normalized;
            Ray ray = new Ray(MuzzlePosition.position, rayPos);
            if (Physics.Raycast(ray, out var hitInfo ,Stat.AttackRange.Val,(int)Layers.HitCollider))
            {
                Debug.DrawRay(MuzzlePosition.position, rayPos * Stat.AttackRange.Val, debugRayColor, 5.0f);
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
        if (!IsReloading && Stat.Capacity.Val > 0 && ((Time.time - lastAttackTime) >= Stat.AttackRate.Val))
        {
            return true;
        }
        return false;
    }
    public virtual void Reload()
    {
        if (Stat.Capacity.Val < Stat.Capacity.MaxVal && !IsReloading)
        {
            IsReloading = true;
            StartCoroutine(ReloadRoutine());
        }
    }

    protected virtual IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(3f);
        Stat.Capacity.Val = Stat.Capacity.MaxVal;
        IsReloading = false;
    }
}
