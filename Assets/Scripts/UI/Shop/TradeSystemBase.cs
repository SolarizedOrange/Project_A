using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TradeSystemBase : MonoBehaviour
{
    [Header("Trade System Settings")]
    public List<ItemBase> ItemList;

    [Header("Trade System Info")]
    public List<TradeSlot> Slots;

    protected PlayerController player;
    public virtual void Init() { player = GameManager.Instance.Player; }

	public virtual void GenerateItemList()
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            var last = ItemList.Count - i;
            if (last<= 0)
                break;

            var randomIndex = Random.Range(0, last);
            // SO Item Instancing 
            Slots[i].Item = Instantiate(ItemList[randomIndex]);
            var item = ItemList[last-1];
            ItemList[last-1] = ItemList[randomIndex];
            ItemList[randomIndex] = item;
        }
    }

    protected virtual bool CanBuyItem(ItemBase item,bool isResell = false) 
    { 
        var price = isResell ? item.ResellPrice : item.Price;
        var otherPrice = isResell ? item.OtherResellPrice : item.OtherPrice;
        return price<= player.Money.Value && otherPrice <= player.XP; 
    }

    protected void BuyItem(ItemBase item, bool isResell = false)
	{
		player.Money.Value -= isResell ? item.ResellPrice : item.Price;
	}

    protected void SellItem(ItemBase item, bool isResell = false)
	{
		player.Money.Value += isResell ? item.ResellPrice : item.Price;
	}
    public virtual void OnCompleteSelect() {}
}
