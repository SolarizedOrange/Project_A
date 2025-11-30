using UnityEngine;

public abstract class RangedWeapon: WeaponBase
{
    public Transform MuzzlePosition;

    [Header("Test Ray Setting")]
    [SerializeField] Color debugRayColor = Color.red;

    public void Fire()
    {
        if (Stat.Capacity.Val <= 0)
        {
            Debug.Log("Out of Ammo");
            return;
        }

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

    public abstract void Reload();
}
