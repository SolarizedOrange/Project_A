using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem",menuName ="Inventory/WeaponItem")]
public class InventoryWeaponItem : ItemBase
{
    [Header("Weapon Info")]
	public WeaponType WeaponType;
}
