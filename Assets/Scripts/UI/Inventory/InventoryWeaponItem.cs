using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem",menuName ="Inventory/WeaponItem")]
public class InventoryWeaponItem : InventoryItemBase
{
    [Header("Weapon Info")]
	public WeaponType WeaponType;
}
