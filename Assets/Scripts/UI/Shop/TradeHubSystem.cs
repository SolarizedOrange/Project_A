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

    public List<ShopItem> GetSelectedItems()
	{
        List<ShopItem> items = new ();
        foreach (var slot in perkSystem.CurrentSelectSlots)
        {
            items.Add(slot.Item);
        }
        foreach (var slot in shopSystem.CurrentSelectSlots)
        {
            items.Add(slot.Item);
        }
        return items;
	}
}
