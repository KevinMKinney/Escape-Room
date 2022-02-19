using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region attributes
    private List<Item> items = new List<Item>();
    private Item equippedItem = null;
    #endregion

    #region methods

    public List<Item> GetItems()
    {
        return items;
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }

    public void DropItem(Item item)
    {
        items.Remove(item);
    }

    public Item GetEquippedItem()
    {
        return equippedItem;
    }

    public void EquipItem(Item item)
    {
        equippedItem = item;
    }

    public void PackUpItem()
    {
        equippedItem = null;
    }
    #endregion

    public void Awake()
    {
        // tests:
        Item gun = new Item("Gun", "A shiny new gun");
        gun.LongDescription = "I found this gun in the room I woke up in. It is shiny and appears to be brand new.";
        AddItem(gun);
    }
}

