using System.Linq;
using UnityEngine;

public class PerkSystem : TradeSystemBase
{
    [Header("PerkSystem Settings")]
    public TradeSlot CurrentSelectSlot;

    public override void Init()
	{
        base.Init();
		Slots = new ();
        Slots = GetComponentsInChildren<TradeSlot>().ToList();
        foreach (var slot in Slots)
        {
            slot.SlotButton.onClick.AddListener(() =>
            {
                if (CanBuyItem(slot.Item) == false)
                    return;

                if (CurrentSelectSlot != null)
                    CurrentSelectSlot.SlotButton.interactable = true;
                CurrentSelectSlot = slot;
                slot.SlotButton.interactable = false;
            });
        }
	}
	public override void OnCompleteSelect()
	{
        if (CurrentSelectSlot != null && CurrentSelectSlot.Item != null
            && CurrentSelectSlot.Item is PerkItem item)
            player.AddPerk(item);
	}
}
