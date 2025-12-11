using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem",menuName ="Shop/Item")]
public class ItemBase : ScriptableObject
{
    [Header("Item Info")]
	public int ID;
	public string ItemName;
	public string Description;
	public Sprite ItemSprite;
	
	[Header("Shop Item Info")]
	public int Price;
	public int ResellPrice;
	public int OtherPrice;
	public int OtherResellPrice;
}