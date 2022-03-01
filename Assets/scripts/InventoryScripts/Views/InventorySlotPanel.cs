using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotPanel : MonoBehaviour
{
    public int itemInFocus;
    public GameObject inventorySlotPanel;
    public GameObject inventoryPanel;
    public GameObject inventoryWrapper;
    public InventoryControl inventoryController;
    public List<Item> itemList;

    // init locates the game objects that are relavent to the InventorySlotPanel views
    public void init()
    {
        inventorySlotPanel = this.gameObject;
        inventoryPanel = inventorySlotPanel.transform.parent.gameObject;
        inventoryWrapper = inventoryPanel.transform.parent.gameObject;
        inventoryController = inventoryWrapper.GetComponent<InventoryControl>();
    }

    public void GetItemList()
    {
        itemList = inventoryController.GetItems();
    }

    public void AssignItems()
    {
        for (int i = 0; i < Inventory.maxItemCount; i++)
        {
            if (i < itemList.Count)
            {
                inventorySlotPanel.transform.GetChild(i).GetComponent<InventorySlot>().Assign(itemList[i]);
                Debug.Log(itemList[i].ItemName);
            } 
            else
            {
                inventorySlotPanel.transform.GetChild(i).GetComponent<InventorySlot>().Assign(null);
            }
        }
    }
}
