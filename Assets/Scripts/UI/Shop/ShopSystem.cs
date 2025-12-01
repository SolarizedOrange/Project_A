using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : TradeSystemBase
{
    [Header("Shop System Settings")]
    public int ItemCount = 5;
    [SerializeField] TradeSlot shopSlotPrefab;
    [SerializeField] Transform shopItemContent;
    [SerializeField] Transform cartItemContent;

	public override void GenerateItemList()
	{
        for (int i = 0; i < Slots.Count; i++)
        {
            Slots[i].SlotButton.onClick.RemoveAllListeners();
            // TODO: Object Pooling
            Destroy(Slots[i].gameObject);
        }
        Slots.Clear();

		for (int i = 0; i < ItemCount; i++)
        {
            // TODO: Object Pooling
            var slot = Instantiate(shopSlotPrefab,shopItemContent);
            slot.SlotButton.onClick.AddListener(() =>
            {
                if (CanSelectSlot() == false)
                    return;
                if (slot.transform.parent == shopItemContent)
                {
                    slot.transform.SetParent(cartItemContent);
                    CurrentSelectSlots.Add(slot);
                }
                else
                {
                    slot.transform.SetParent(shopItemContent);
                    CurrentSelectSlots.Remove(slot);
                }
            });

            Slots.Add(slot);
        }

        base.GenerateItemList();
	}
}
