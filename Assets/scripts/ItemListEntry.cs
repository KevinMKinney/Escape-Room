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

    /// <summary>
    /// OnPointerEnter() handles the color change when the mouse hovers over the list entry.
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerEnter(PointerEventData data)
    {
        itemText.color = new Color32(214, 48, 49, 255);
    }

    /// <summary>
    /// OnPointerExit() handles the color change when the mouse no longer hovers over
    /// the list entry
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerExit(PointerEventData data)
    {
        itemText.color = new Color32(0, 0, 0, 255);
    }

    /// <summary>
    /// OnPointerClick() handles the functionality when a list entry is clicked.
    /// When a list entry is clicked, the selected item is set in the inventory and
    /// the item display section of the 'items' notebook page is updated with the
    /// selected items information. Controls are also shown
    /// </summary>
    /// <param name="data"></param>
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
