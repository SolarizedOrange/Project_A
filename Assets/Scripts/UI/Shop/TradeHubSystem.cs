using System.Collections.Generic;
using UnityEngine;

public class TradeHubSystem : MonoBehaviour
{
    [SerializeField] TradeSystemBase perkSystem;
    [SerializeField] ShopSystem shopSystem;
    [SerializeField] List<ShopItem> perkItems;
    [SerializeField] List<ShopItem> shopItems;

	void OnEnable()
	{
        perkSystem.ItemList = perkItems;
        shopSystem.ItemList = shopItems;

		perkSystem.GenerateItemList();
		shopSystem.GenerateItemList();

        perkSystem.gameObject.SetActive(true);
        shopSystem.gameObject.SetActive(false);
	}

	void OnDisable()
	{
		perkItems.Clear();
        shopItems.Clear();
	}

	public List<ItemBase> GetBuyItemInstance()
	{
        List<ItemBase> items = new ();
        foreach (var slot in perkSystem.CurrentSelectSlots)
        {
            items.Add(Instantiate(slot.Item.BuyItem));
        }
        foreach (var slot in shopSystem.CurrentSelectSlots)
        {
            items.Add(Instantiate(slot.Item.BuyItem));
        }
        return items;
	}
}
