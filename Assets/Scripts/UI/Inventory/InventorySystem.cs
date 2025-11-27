using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public struct InventorySlot
{
    public InventoryItemBase SlotItem;
    public Image SlotImage;
}

public class InventorySystem : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] List<Image> slotObjects;
    [SerializeField] InputActionMap actionMap;

    List<InventorySlot> slots;
    List<System.Action<InputAction.CallbackContext>> handlers = new ();
    void OnEnable()
    {
        actionMap.Enable();
        for(int i = 0; i < actionMap.actions.Count; i++)
		{
            int idx = i;
            System.Action<InputAction.CallbackContext> handler = (callback) => OnSelectItem(idx);
            handlers.Add(handler);
            actionMap.actions[i].performed += handler;
		}
    }
    void OnDisable()
    {
        for(int i = 0; i < actionMap.actions.Count; i++)
		{
            actionMap.actions[i].performed -= handlers[i];
		}
        actionMap.Disable();
    }

	void Awake()
	{
        slots = new ();
        foreach (var slot in slotObjects)
		{
            // null slot image disabled
            slot.enabled = slot.sprite;
			slots.Add(new InventorySlot(){SlotItem = null, SlotImage = slot});
		}
	}

    public InventoryItemBase GetSlotItem(int idx)
	{
		if (slots != null && slots.Count > idx && idx >= 0)
            return slots[idx].SlotItem;
        else 
            return null;
	}

    public bool SetSlotItme(int idx, InventoryItemBase item,bool isOverride = true)
	{
		if (slots.Count <= idx || idx < 0) return false;
        if (isOverride == false && slots[idx].SlotItem != null) return false;

        var slot = slots[idx];
        slot.SlotItem = item;
        var img = slot.SlotImage;
        img.sprite = item.ItemSprite;
        img.enabled = img.sprite;
        slots[idx] = slot;
        return true;
	}

    void OnSelectItem(int idx)
	{
        // TODO: Select Method
		Debug.Log($"Select Item: {idx} Can Select Item: {GetSlotItem(idx)}");
	}
}
