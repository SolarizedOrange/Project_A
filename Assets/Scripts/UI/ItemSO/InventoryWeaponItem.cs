using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem",menuName ="Shop/WeaponItem")]
public class InventoryWeaponItem : ItemBase
{
    [Header("Weapon Info")]
	public WeaponBase WeaponPref;
}
