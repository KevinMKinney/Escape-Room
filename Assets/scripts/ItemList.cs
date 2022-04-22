using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemList : MonoBehaviour
{
    private GameObject itemList;
    
    // Start is called before the first frame update
    void Start()
    {
        itemList = this.gameObject;
    }

    public void UpdateList(Inventory inventory)
    {
        List<Item> items = inventory.GetItems();
        GameObject itemSlot;
        for (int i = 0; i < Inventory.maxItemCount; i++)
        {
            itemSlot = itemList.transform.GetChild(i).gameObject;
            
            if (i < items.Count)
            {
                if (i == inventory.GetEquippedItemIndex())
                {
                    itemSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = $"** { items[i].ItemName} ({ items[i].ShortDescription})";
                } else
                {
                    itemSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = $"{items[i].ItemName} ({items[i].ShortDescription})";
                }
            } else
            {
                itemSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
            }
        }
    }
}
