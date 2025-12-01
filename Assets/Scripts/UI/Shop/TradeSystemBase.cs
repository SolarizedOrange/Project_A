using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TradeSystemBase : MonoBehaviour
{
    [Header("Trade System Settings")]
    public List<ShopItem> ItemList;

    [Header("Puck System Info")]
    public HashSet<TradeSlot> CurrentSelectSlots;
    public List<TradeSlot> Slots;
	void Awake()
	{
		Init();
	}

	void OnDestroy()
	{
		foreach (var slot in Slots)
        {
            slot.SlotButton.onClick.RemoveAllListeners();
        }
	}

    protected virtual void Init() { CurrentSelectSlots = new ();}

	public virtual void GenerateItemList()
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            var last = ItemList.Count - i;
            if (last<= 0)
                break;

            var randomIndex = Random.Range(0, last);
            Slots[i].Item = ItemList[randomIndex];
            var item = ItemList[last-1];
            ItemList[last-1] = ItemList[randomIndex];
            ItemList[randomIndex] = item;
        }
    }

    protected bool CanSelectSlot()
	{
		// TODO: Perk Item Price Check
        return true;
	}
}
