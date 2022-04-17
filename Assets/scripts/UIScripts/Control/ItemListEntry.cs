using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemListEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    TMP_Text itemText;
    Inventory inventory;
    ItemDisplay itemDisplay;
    ItemControls itemControls;

    // Start is called before the first frame update
    void Start()
    {
        itemText = this.gameObject.GetComponent<TMP_Text>();
        inventory = GameObject.FindGameObjectWithTag("items").GetComponent<Inventory>();
        itemDisplay = GameObject.Find("ItemDisplay").GetComponent<ItemDisplay>();
        itemControls = GameObject.Find("ItemControls").GetComponent<ItemControls>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        itemText.color = new Color32(214, 48, 49, 255);
    }

    public void OnPointerExit(PointerEventData data)
    {
        itemText.color = new Color32(0, 0, 0, 255);
    }

    public void OnPointerClick(PointerEventData data)
    {
        int itemSlotIndex = this.gameObject.transform.parent.GetSiblingIndex();
        inventory.SelectItem(itemSlotIndex);
        itemDisplay.UpdateItemDisplay(inventory.GetSelectedItem());
    }
}
