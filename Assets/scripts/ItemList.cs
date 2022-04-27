using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemList : MonoBehaviour
{
    // attributes:
    private GameObject itemList;
    
    // Start is called before the first frame update
    void Start()
    {
        // declaration of attribtues:
        itemList = GameObject.Find("ItemList");
    }

    // UpdateList() takes an inventory as the input and updates the
    // displayed item list entries with the current inventory of items.
    // it also displays the currently equipped item within the inventory
    public void UpdateList(Inventory inventory)
    {
        // get the items in the inventory
        List<Item> items = inventory.GetItems();
        GameObject itemSlot;

        // iterate through the inventory items
        for (int i = 0; i < Inventory.maxItemCount; i++)
        {
            // get the item slot in the item list
            itemSlot = itemList.transform.GetChild(i).gameObject;
            
            if (i < items.Count)
            {
                // this is an item list entry that should display actual item text
                if (i == inventory.GetEquippedItemIndex())
                {
                    // mark item as equipped
                    itemSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = $"** { items[i].ItemName} ({ items[i].ShortDescription})";
                } else
                {
                    itemSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = $"{items[i].ItemName} ({items[i].ShortDescription})";
                }
            } else
            {
                // there is no item in the inventory for this item slot
                itemSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
            }
        }
    }
}
