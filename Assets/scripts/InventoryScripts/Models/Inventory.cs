using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region attributes
    private List<Item> items = new List<Item>(); // list of items in inventory
    private int equippedItemIndex = -1; // -1 indicates that no item is equipped
    public static int maxItemCount = 5;
    #endregion

    #region methods
    // Get the items currently in the inventory
    public List<Item> GetItems()
    {
        return items;
    }

    // Add an item to the inventory
    public void AddItem(Item item)
    {
        if (items.Count < maxItemCount)
        {
            items.Add(item);
        }
    }

    // Remove an item from the inventory
    public void DropItem(int i)
    {
        items.RemoveAt(i);
    }

    // Return the index of the item that is currently equipped
    public int GetEquippedItemIndex()
    {
        return equippedItemIndex;
    }

    // Assign an item to be equipped
    public int EquipItem(int i)
    {
        if (i >= 0 || i < items.Count) equippedItemIndex = i;
        return equippedItemIndex;
    }

    // Unequip item
    public void PackUpItem()
    {
        equippedItemIndex = -1;
    }

    // initTests() initializes the inventory with some test items
    public void initTests()
    {
        Item gun = new Item("Gun", "A shiny new gun.");
        Item gasCan = new Item("Gas Can", "A full can of gas.");
        Item mazeBoard = new Item("Maze Board", "A maze enclosed in glass.");
        mazeBoard.LongDescription = "I found this 3D maze enclosed in glass in one of the rooms here. It looks like there is a button at the center of the maze. But how do I press it?";
        AddItem(gun);
        AddItem(gasCan);
        AddItem(mazeBoard);
        EquipItem(0);
    }
    #endregion
}

