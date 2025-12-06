using System.Linq;

public class PerkSystem : TradeSystemBase
{
    protected override void Init()
	{
        base.Init();
		Slots = new ();
        Slots = GetComponentsInChildren<TradeSlot>().ToList();
        foreach (var slot in Slots)
        {
            slot.SlotButton.onClick.AddListener(() =>
            {
                if (CanSelectSlot() == false)
                    return;

                foreach (var s in CurrentSelectSlots)
                {
                    s.SlotButton.interactable = true;
                }
                CurrentSelectSlots.Clear();

                CurrentSelectSlots.Add(slot);

                slot.SlotButton.interactable = false;
            });
        }
	}
}
