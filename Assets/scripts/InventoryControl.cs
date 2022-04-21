using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryControl : MonoBehaviour
{
    private Inventory inventory;
    private ItemList itemList;
    private ItemControls itemControls;

    // Start is called before the first frame update
    void Start()
    {
        inventory = this.gameObject.GetComponent<Inventory>();
        //inventory.StartingInventory();

        itemList = GameObject.Find("ItemList").GetComponent<ItemList>();
        itemList.UpdateList(inventory);

        itemControls = GameObject.Find("ItemControls").GetComponent<ItemControls>();
    }
}
