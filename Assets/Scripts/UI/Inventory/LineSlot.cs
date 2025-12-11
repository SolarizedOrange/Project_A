using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineSlot : MonoBehaviour, IPointerEnterHandler,IDropHandler,IPointerExitHandler
{
    [Header("LineSlot Setting")]
    Image lineImage;
    InventorySystem Inventory;

	void Start()
	{
		lineImage = GetComponent<Image>();
        Inventory = UIController.Instance.Inventory;
	}

	public void OnDrop(PointerEventData eventData)
	{
		var obj = eventData.pointerDrag;
        obj.transform.SetParent(transform.parent);
        obj.transform.SetSiblingIndex(transform.GetSiblingIndex()); 
        Inventory.UpdateWeaponSlot();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
        if (eventData.pointerDrag == null)
		{
			lineImage.color = lineImage.color = new Color(0,0,0,0);
		}
		else
		{
		    lineImage.color = Color.white;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
        lineImage.color = new Color(0,0,0,0);
	}
}
