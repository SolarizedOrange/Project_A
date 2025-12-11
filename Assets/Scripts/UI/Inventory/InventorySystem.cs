using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public struct QuickSlot
{
    public InventoryWeaponSlot InventorySlot;
    public Image SlotImage;
}

public class InventorySystem : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] GameObject playerInfo;
    [SerializeField] List<Image> quickSlotObjects;
    [SerializeField] InputActionMap inventoryActionMap;
    [SerializeField] InputActionMap slotActionMap;

    PlayerController player;
    List<Action<InputAction.CallbackContext>> slotHandlers = new ();
    Action<InputAction.CallbackContext> inventoryHandler;

    bool canUseQuickSlot = true;
    bool canUseInventory = true;
    void InitInput()
    {
        inventoryActionMap.Enable();
        for (int i = 0; i< inventoryActionMap.actions.Count; i++)
		{
            inventoryHandler = (callback) => 
            { 
                if (canUseInventory == false) return;
                playerInfo.SetActive(!playerInfo.activeSelf);
                canUseQuickSlot = !playerInfo.activeSelf;
            };
            inventoryActionMap.actions[i].performed += inventoryHandler;
		}

        slotActionMap.Enable();
        for(int i = 0; i < slotActionMap.actions.Count; i++)
		{
            int idx = i;
            Action<InputAction.CallbackContext> handler = (callback) => OnSelectItem(idx);
            slotHandlers.Add(handler);
            slotActionMap.actions[i].performed += handler;
		}
    }
    void OnDestroy()
    {
        for (int i = 0; i< inventoryActionMap.actions.Count; i++)
		{
			inventoryActionMap.actions[i].performed -= inventoryHandler;
		}
        for(int i = 0; i < slotActionMap.actions.Count; i++)
		{
            slotActionMap.actions[i].performed -= slotHandlers[i];
		}
        slotActionMap.Disable();
    }

	void Start()
	{
        player = GameManager.Instance.Player;
        InitInput();
        InitQuickSlot();
        InitWeaponSlot();
	}

    void OnSelectItem(int idx)
	{
        if (canUseInventory == false || canUseQuickSlot == false) return;

        // TODO: Select Method
		Debug.Log($"Select Item: {idx} Can Select Item: {GetQuickSlot(idx)}");
        var slot = GetQuickSlot(idx);
        if (player != null)
		{
			player.EquipWeapon(slot != null ? slot.InstanceItem : null);
            player.GetComponent<PlayerCombat>().OnWeaponSwap();
		}
    }

    public void SetInventoryInput(bool isEnable)
	{
		canUseInventory = isEnable;
	}

#region QuickSlot
    List<QuickSlot> quickSlots;
    void InitQuickSlot()
	{
		quickSlots = new ();
        foreach (var slot in quickSlotObjects)
		{
            // null slot image disabled
            slot.enabled = slot.sprite != null;
			quickSlots.Add(new QuickSlot(){InventorySlot = null, SlotImage = slot});
		}
	}

    public InventoryWeaponSlot GetQuickSlot(int idx)
	{
        if (quickSlots.Count > idx && idx >= 0)
            return quickSlots[idx].InventorySlot;
        else 
            return null;
	}

    public void UpdateQuickSlot()
	{
        for (int i = 0; i< WeaponSlots.Count; i++)
		{
		    if (quickSlots.Count <= i) break;
            var quickSlot = quickSlots[i];
            var img = quickSlot.SlotImage;
            quickSlot.InventorySlot = WeaponSlots[i];
            var slot = quickSlot.InventorySlot;
            if (slot != null && slot.Item != null)
                img.sprite = slot.Item.ItemSprite;
            else
                img.sprite = null;
            img.enabled = img.sprite != null;
            quickSlots[i] = quickSlot;
        }
	}
#endregion

#region WeaponSlot
    public List<InventoryWeaponSlot> WeaponSlots;
    [SerializeField] List<LineSlot> lineSlots;
    
    void InitWeaponSlot()
	{
		WeaponSlots = GetComponentsInChildren<InventoryWeaponSlot>(true).ToList();
        lineSlots = GetComponentsInChildren<LineSlot>(true).ToList();
        UpdateWeaponSlot();
	}

    public void DisableLineSlot(int currentWeponSLotidx)
	{
		lineSlots[(currentWeponSLotidx-1)/2].gameObject.SetActive(false);
	}

    public void UpdateWeaponSlot()
	{
		for (int i = 0; i < lineSlots.Count; i++)
        {
            lineSlots[i].gameObject.SetActive(true);
            lineSlots[i].transform.SetSiblingIndex(2*i);
        }

        WeaponSlots.Sort((a,b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
        UpdateQuickSlot();
	}
#endregion
}
