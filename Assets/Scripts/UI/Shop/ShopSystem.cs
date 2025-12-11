using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : TradeSystemBase
{
    [Header("Weapon Shop Settings")]
    public int ItemCount = 3;
    [SerializeField] TradeSlot weaponSlotPrefab;
    [SerializeField] Transform inventoryItemContent;
    [SerializeField] Transform weaponshopItemContent;
    InventorySystem inventory;

    public override void Init()
	{
		base.Init();
        inventory = UIController.Instance.Inventory;
        InitWeaponItem();
	}

    #region Weapon
    void InitWeaponItem()
	{
		var weaponSlots = inventory.WeaponSlots;
        for (int i = 0; i < weaponSlots.Count; i++)
		{
            if (weaponSlots[i].Item == null) continue;
			// TODO: Object Pooling
            var slot = Instantiate(weaponSlotPrefab,inventoryItemContent);

            slot.SlotButton.onClick.AddListener(() => OnClickShopButton(slot,true));
            slot.Item = weaponSlots[i].Item;
        }
	}
	public override void GenerateItemList()
	{
		for (int i = 0; i < ItemCount; i++)
        {
            // TODO: Object Pooling
            var slot = Instantiate(weaponSlotPrefab,weaponshopItemContent);
            slot.SlotButton.onClick.AddListener(() => OnClickShopButton(slot));

            Slots.Add(slot);
        }

        base.GenerateItemList();
	}

    void OnClickShopButton(TradeSlot slot,bool isResell = false)
	{
		if (slot.transform.parent == weaponshopItemContent
            && CanBuyItem(slot.Item,isResell) && TryFindBlankWeaponSlot(slot.Item, out var slotIdx))
        {
            BuyItem(slot.Item,isResell);
            inventory.WeaponSlots[slotIdx].Item = slot.Item as InventoryWeaponItem;
            inventory.UpdateWeaponSlot();
            slot.transform.SetParent(inventoryItemContent);
        }
        else if (slot.transform.parent == inventoryItemContent)
        {
            SellItem(slot.Item,isResell);
            if (TryFindWeaponSlot(slot.Item, out var findIdx))
            {
                inventory.WeaponSlots[findIdx].Item = null;
                inventory.UpdateWeaponSlot();
            }
            slot.transform.SetParent(weaponshopItemContent);
        }
	}

    bool TryFindBlankWeaponSlot(ItemBase item, out int slotIdx)
	{
		slotIdx = -1;
        if (item is not InventoryWeaponItem weaponItem) return false;
        var slots = inventory.WeaponSlots;
        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i].Item != null) continue;
            if (slots[i].IsPistolSlot && weaponItem.WeaponPref.GetWeaponType() != WeaponType.Handgun) continue;
            slotIdx = i;
            return true;
        }
        return false;
	}

    bool TryFindWeaponSlot(ItemBase item, out int slotIdx)
	{
        slotIdx = -1;
        if (item is not InventoryWeaponItem) return false;
        var slots = inventory.WeaponSlots;
        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i].Item == item)
			{
                slotIdx = i;
				return true;
			}
        }
        return false;
	}

	public override void OnCompleteSelect()
	{
		foreach (var item in inventory.WeaponSlots)
        {
            item.UpdateInstanceItem();
        }
	}
    #endregion
}
