using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region attributes
    private List<Item> items = new List<Item>();
    private Item equippedItem = null;
    private int maxItemCount = 5;
    #endregion

    #region methods

    public List<Item> GetItems()
    {
        return items;
    }

    public void AddItem(Item item)
    {
        if (items.Count < maxItemCount)
        {
            items.Add(item);
        }
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

    public int GetMaxItemCount()
    {
        return maxItemCount;
    }
    #endregion

    public void Awake()
    {
        // tests:
        Item gun = new Item("Gun", "A shiny new gun.");
        Item gasCan = new Item("Gas Can", "A full can of gas.");
        Item mazeBoard = new Item("Maze Board", "A maze enclosed in glass.");
        mazeBoard.LongDescription = "I found this 3D maze enclosed in glass in one of the rooms here. It looks like there is a button at the center of the maze. But how do I press it?";
        AddItem(gun);
        AddItem(gasCan);
        AddItem(mazeBoard);

        Debug.Log("Inventory Initialized...");
    }
}

