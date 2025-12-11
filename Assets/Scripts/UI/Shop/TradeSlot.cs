using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class TradeSlot : MonoBehaviour
{
    [Header("Trade Slot Settings")]
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI decriptionText;
    public Button SlotButton;

    public bool IsResellSlot;
    [Header("Trade Slot Item Info")]
    [SerializeField] ItemBase item;
    public ItemBase Item
	{
		get
		{
			return item;
		}
        set
		{
            item = value;
            if (value != null)
			{
                itemImage.sprite = item.ItemSprite;
                nameText.text = item.ItemName;
                decriptionText.text = item.Description;
                UpdatePriceText();
			}
		}
	}

    void UpdatePriceText()
	{
        if (IsResellSlot)
		{
			if (Item is InventoryWeaponItem)
            {
                priceText.text = item.ResellPrice.ToString();
            }
            else
			{
				priceText.text = item.OtherPrice.ToString();
			}
		}
        else
		{
			if (item is InventoryWeaponItem)
			{
				priceText.text = item.Price.ToString();
			}
            else
			{
				priceText.text = item.OtherPrice.ToString();
			}
		}
		
	}
}
