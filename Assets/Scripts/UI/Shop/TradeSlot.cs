using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class TradeSlot : MonoBehaviour
{
    [Header("Trade Slot Settings")]
    [SerializeField] Image ItemImage;
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI PriceText;
    [SerializeField] TextMeshProUGUI DecriptionText;
    public Button SlotButton;

    [Header("Puck Slot Item Info")]
    [SerializeField] ShopItem item;
    public ShopItem Item
	{
		get
		{
			return item;
		}
        set
		{
			if (value != null)
            {
                item = value;
                ItemImage.sprite = item.ItemSprite;
                NameText.text = item.ItemName;
                PriceText.text = item.OtherPrice.ToString();
                DecriptionText.text = item.Description;
            }
		}
	}
}
