using UnityEngine;

public abstract class RangedWeapon: WeaponBase
{
    public Transform MuzzlePosition;

    public void Fire()
    {
        Debug.Log("FIRE");
        if (Physics.Raycast(MuzzlePosition.position, MuzzlePosition.forward, out var hitInfo))
        {
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
