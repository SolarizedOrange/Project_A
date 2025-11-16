using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public abstract void Attack();
    public abstract WeaponType GetWeaponType();
}
