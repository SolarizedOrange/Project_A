using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem",menuName ="Shop/ShopItem")]
public class ShopItem : ItemBase
{
    [Header("Shop Item Info")]
	public int Price;
	// Add Other Price
	public int OtherPrice;
	public ItemBase BuyItem;
}
