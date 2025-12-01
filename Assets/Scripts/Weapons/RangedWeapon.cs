using UnityEngine;

public abstract class RangedWeapon: WeaponBase
{
    public Transform MuzzlePosition;

    [Header("Test Ray Setting")]
    [SerializeField] Color debugRayColor = Color.red;

    public override bool Attack(bool hasJustAttacked)
    {
        if (CanAttack())
        {
            Fire();
            lastAttackTime = Time.time;
            return true;
        }
        return false;
    }
    public void Fire()
    {
        Debug.Log("FIRE");
        Stat.Capacity.Val--;
        Ray ray = new Ray(MuzzlePosition.position, MuzzlePosition.forward);
        if (Physics.Raycast(ray, out var hitInfo ,Stat.AttackRange.Val,(int)Layers.HitCollider))
        {
            Debug.DrawRay(MuzzlePosition.position, MuzzlePosition.forward * Stat.AttackRange.Val, debugRayColor, 5.0f);
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
    public override bool CanAttack()
    {
        if (Stat.Capacity.Val > 0 && ((Time.time - lastAttackTime) >= Stat.AttackRate.Val))
        {
            return true;
        }
        return false;
    }
    public virtual void Reload()
    {
        if (Stat.Capacity.Val < Stat.Capacity.MaxVal)
        {
            Stat.Capacity.Val = Stat.Capacity.MaxVal;
        }
    }
}
