using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryControl : MonoBehaviour
{
    public GameObject inventoryWrapper;
    public GameObject inventoryPanel;
    public Inventory inventory;
    public InventorySlotPanel inventorySlotPanel;
    public InventoryDisplayPanel inventoryDisplayPanel;
    
    // init() locates the game objects that this script will
    // interact with
    public void init()
    {
        // locate the in game objects representing the inventory and UI objects
        inventoryWrapper = this.gameObject;
        inventoryPanel = inventoryWrapper.transform.Find("InventoryPanel").gameObject;
        inventory = inventoryWrapper.GetComponent<Inventory>();
        inventorySlotPanel = inventoryPanel.transform.Find("SlotPanel").GetComponent<InventorySlotPanel>();
        inventoryDisplayPanel = inventoryPanel.transform.Find("DisplayPanel").GetComponent<InventoryDisplayPanel>();
    }

    // Add an item to the inventory, and return an un updated item list
    public List<Item> Add(Item item)
    {
        inventory.AddItem(item);
        return inventory.GetItems();
    }

    // Drop an item to the inventory, and return an unpdate item list
    public List<Item> Drop(int index)
    {
        inventory.DropItem(index);
        return inventory.GetItems();
    }

    // Equip an item based on the index in the inventory, and return
    // that index
    public int Equip(int index)
    {
        inventory.EquipItem(index);
        return inventory.GetEquippedItemIndex();
    }

    // Put away the currently equipped item
    public void PutAway()
    {
        inventory.PackUpItem();
    }

    // Get a copy of the current item list
    public List<Item> GetItems()
    {
        return inventory.GetItems();
    }

    // Update is called once per frame
    void Update()
    {
        // toggle inventory visibility when user presses "Inventory" button 'i'
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            if (inventoryPanel.activeSelf)
            {
                inventorySlotPanel.GetItemList();
                inventorySlotPanel.AssignItems();
            }
        }
    }
}
