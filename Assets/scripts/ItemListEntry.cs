using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemListEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // attributes:
    TMP_Text itemText;
    Inventory inventory;
    ItemDisplay itemDisplay;
    ItemControls itemControls;

    // Start is called before the first frame update
    void Start()
    {
        // declaration of attributes:
        itemText = this.gameObject.GetComponent<TMP_Text>();
        inventory = GameObject.FindGameObjectWithTag("items").GetComponent<Inventory>();
        itemDisplay = GameObject.Find("ItemDisplay").GetComponent<ItemDisplay>();
        itemControls = GameObject.Find("ItemControls").GetComponent<ItemControls>();
    }

    // OnPointerEnter() handles the color change when the mouse hovers over the list entry.
    public void OnPointerEnter(PointerEventData data)
    {
        itemText.color = new Color32(214, 48, 49, 255);
    }

    // OnPointerExit() handles the color change when the mouse no longer hovers over
    // the list entry
    public void OnPointerExit(PointerEventData data)
    {
        itemText.color = new Color32(0, 0, 0, 255);
    }

    // OnPointerClick() handles the functionality when a list entry is clicked.
    // When a list entry is clicked, the selected item is set in the inventory and
    // the item display section of the 'items' notebook page is updated with the 
    // selected items information. The item controls ('equip', 'drop', etc.) are
    // also displayed
    public void OnPointerClick(PointerEventData data)
    {
        // update the selected item in the inventory
        int itemSlotIndex = this.gameObject.transform.parent.GetSiblingIndex();
        inventory.SelectItem(itemSlotIndex);

        // update the item display field with the selected item information
        itemDisplay.UpdateItemDisplay(inventory.GetSelectedItem());

        // display the item inventory controls
        itemControls.ShowControls();
    }
}
