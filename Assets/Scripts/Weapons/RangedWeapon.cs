using UnityEngine;

public abstract class RangedWeapon: WeaponBase
{
    public Transform MuzzlePosition;

    public override void Attack()
    {

        if (Physics.Raycast(MuzzlePosition.position, MuzzlePosition.forward, out var hitInfo))
        {
            var res = hitInfo.collider;
            if (res.CompareTag("Enemy"))
            {
                // res.GetComponent<EnemyController>().OnHit();
            }
        }
    }

}
