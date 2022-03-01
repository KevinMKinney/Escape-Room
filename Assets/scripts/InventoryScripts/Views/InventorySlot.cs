using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public int slotNumber;
    public Item item = null;
    public GameObject slotPanel;
    public GameObject inventorySlot;
    public GameObject slotName;
    public GameObject slotDescription;
    public GameObject slotIcon;

    // init() locates the game objects relavent to InventorySlot views
    public void init()
    {
        inventorySlot = this.gameObject;
        slotPanel = inventorySlot.transform.parent.gameObject;
        slotName = inventorySlot.transform.Find("SlotName").gameObject;
        slotDescription = inventorySlot.transform.Find("SlotDescription").gameObject;
        slotIcon = inventorySlot.transform.Find("SlotIcon").gameObject;

        // set the styling for the slotName
        slotName.GetComponent<UnityEngine.UI.Text>().fontSize = 18;

        // set the styling for the slotDescription
        slotDescription.GetComponent<UnityEngine.UI.Text>().fontSize = 14;
    }

    // UpdateSlotUI() updates the text/image values of the itemName, itemDescription, and
    // itemImage objects
    public void UpdateSlotUI()
    {
        if (item == null)
        {
            // no item in this slot
            slotName.GetComponent<UnityEngine.UI.Text>().text = "-";
            slotDescription.GetComponent<UnityEngine.UI.Text>().text = "-";
        }
        else
        {
            slotName.GetComponent<UnityEngine.UI.Text>().text = item.ItemName;
            slotDescription.GetComponent<UnityEngine.UI.Text>().text = item.ShortDescription;
        }
    }

    // Assign() assigns an item to this item slot, and then initializes the update UI process
    public void Assign(Item item)
    {
        this.item = item;
        UpdateSlotUI();
    }
}
