using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryWeaponSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("InventorySlot Setting")]
    Image selfUI;
    InventorySystem Inventory;
    public InventoryWeaponItem Item;
    public bool IsPistolSlot;

    [Header("InventoryItem Info")]
    WeaponBase instanceItemPref;
    public WeaponBase InstanceItem;

    int childIndex;
    Transform originParent;
    void Start()
    {
        selfUI = GetComponent<Image>();
        Inventory = UIController.Instance.Inventory;
    }
    
	public void OnBeginDrag(PointerEventData eventData)
	{
        childIndex = transform.GetSiblingIndex();
        originParent = transform.parent;
        transform.SetParent(Inventory.transform);
        selfUI.raycastTarget = false;
        Inventory.DisableLineSlot(childIndex);
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
        selfUI.raycastTarget = true;
        if (transform.parent != originParent)
		{
			transform.SetParent(originParent);
            transform.SetSiblingIndex(childIndex);
		}
        Inventory.UpdateWeaponSlot();
	}

    public void UpdateInstanceItem()
	{
		if (Item == null || Item.WeaponPref == null || Item.WeaponPref != instanceItemPref)
		{
			Destroy(InstanceItem?.gameObject);
            instanceItemPref = null;
            InstanceItem = null;
		}

        if (Item != null && Item.WeaponPref != null)
		{
			instanceItemPref = Item.WeaponPref;
            InstanceItem = Instantiate(instanceItemPref);
            InstanceItem.gameObject.SetActive(false);
		}
	}
}
