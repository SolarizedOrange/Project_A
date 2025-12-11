using System.Collections.Generic;
using UnityEngine;

public class TradeHubSystem : MonoBehaviour
{
    [SerializeField] TradeSystemBase perkSystem;
    [SerializeField] ShopSystem shopSystem;
    [SerializeField] List<ItemBase> perkItems;
    [SerializeField] List<ItemBase> weaponItems;
    [SerializeField] List<ItemBase> bulletItems;
    [SerializeField] List<ItemBase> armorItems;

	void Start()
	{
        perkSystem.ItemList = perkItems;
        shopSystem.ItemList = weaponItems;
        perkSystem.Init();
        shopSystem.Init();

		perkSystem.GenerateItemList();
		shopSystem.GenerateItemList();

        perkSystem.gameObject.SetActive(true);
        shopSystem.gameObject.SetActive(false);
	}

}
