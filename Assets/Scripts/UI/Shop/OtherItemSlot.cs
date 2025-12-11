using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OtherItemSlot : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] bool isBulletItem;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI valueText;
    [SerializeField] Button buyButton;
    [SerializeField] Button sellButton;

    PlayerController player;
	void Start()
	{
		player = GameManager.Instance.Player;

        //bind
        player.Money.AddListenerWrapper(UpdateMoney);
        if (isBulletItem)
		{
            nameText.SetText(bulletItem.ItemName + " : ");
            priceText.SetText(bulletItem.Price.ToString());
            player.BulletAmmo[bulletItem.BulletType].AddListenerWrapper(UpdateAmmo);
		}
        else
		{
            nameText.SetText(armorItem.ItemName + " : ");
            priceText.SetText(armorItem.Price.ToString());
            player.HasArmor.AddListenerWrapper(UpdateArmor);
		}

        buyButton?.onClick.AddListener(isBulletItem ? BuyBullet:BuyArmor);
        sellButton?.onClick.AddListener(isBulletItem ? SellBullet:SellArmor);
	}

	void OnDestroy()
	{
		player.Money.RemoveListenerWrapper(UpdateMoney);
        if (isBulletItem)
		{
			player.BulletAmmo[bulletItem.BulletType].RemoveListenerWrapper(UpdateAmmo);
		}
        else
		{
			player.HasArmor.RemoveListenerWrapper(UpdateArmor);
		}

        buyButton?.onClick.RemoveAllListeners();
        sellButton?.onClick.RemoveAllListeners();
	}

    void UpdateMoney(int newMoney)
	{
        if (isBulletItem)
		{
		    buyButton.enabled = newMoney >= bulletItem.Price;
		}
        else
		{
            buyButton.enabled = newMoney >= armorItem.Price;
            UpdateArmor(player.HasArmor.Value);
		}
	}

    [Header("Bullet Settings")]
    [SerializeField] BulletItem bulletItem;

    public void BuyBullet()
	{
		player.Money.Value -= bulletItem.Price;
        player.BulletAmmo[bulletItem.BulletType].Value += bulletItem.AmmoPerTrade;
	}

    public void SellBullet()
	{
		player.Money.Value += bulletItem.Price;
        player.BulletAmmo[bulletItem.BulletType].Value -= bulletItem.AmmoPerTrade;
	}


    void UpdateAmmo(int newAmmo)
	{
        if(isBulletItem == false) return;
        sellButton.enabled = newAmmo >= bulletItem.AmmoPerTrade;
		valueText.SetText(newAmmo.ToString());
	}

    [Header("Armor Settings")]
    [SerializeField] ItemBase armorItem;
    public void BuyArmor()
	{
		player.Money.Value -= armorItem.Price;
        player.HasArmor.Value = true;
	}

    public void SellArmor()
	{
		player.Money.Value += armorItem.Price;
        player.HasArmor.Value = false;
	}

    void UpdateArmor(bool newHasArmor)
	{
		if (isBulletItem) return;
        buyButton.enabled = !newHasArmor && player.Money.Value >= armorItem.Price;
        sellButton.enabled = newHasArmor;
		valueText.SetText(newHasArmor? "Equipped" : "Unequipped");
	}
}
