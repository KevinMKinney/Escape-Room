using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region singleton_init
    // initialization of single inventory instance:
    public static Inventory inventoryInstance;

    private void Awake()
    {
        inventoryInstance = this;
    }
    #endregion

    #region attributes
    public List<Item> items = new List<Item>();
    public List<Item> equippedItems = new List<Item>();
    public InventoryUI ui;
    #endregion

    #region methods
    public void AddItem(Item item)
    {
        items.Add(item);
        item.inInventory = true;
    }

    public void DropItem(Item item)
    {
        items.Remove(item);
        item.inInventory = false;
    }

    public void EquipItem(Item item)
    {
        if (!equippedItems.Contains(item))
        {
            equippedItems.Add(item);
        }
    }

    public void PackUpItem(Item item)
    {
        if (equippedItems.Contains(item))
        {
            equippedItems.Remove(item);
        }
    }
    #endregion
}
