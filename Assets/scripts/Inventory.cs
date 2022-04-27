using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region attributes
    private List<Item> items = new List<Item>(); // list of items in inventory
    private int equippedItemIndex = -1; // -1 indicates that no item is equipped
    private int selectedItemIndex = -1; // -1 indicates that no item is selected
    public static int maxItemCount = 8; // inventory can have no more than maxItemCount items
    #endregion

    #region methods

    // Get the items currently in the inventory
    public List<Item> GetItems()
    {
        return items;
    }

    // Add an item to the inventory
    public int AddItem(Item item)
    {
        if (items.Count < maxItemCount)
        {
            items.Add(item);
            return items.Count;
        }

        return -1;
    }

    // Remove an item from the inventory
    public void DropItem(int i)
    {
        if (i >= 0 && i < items.Count)
        {
            Item current = items[i];
            items.RemoveAt(i);

            // handle an equipped item being removed:
            if (equippedItemIndex == i)
            {
                equippedItemIndex = -1;
            }
        }
    }

    // select an item (only if within range of the actual item count)
    public int SelectItem(int i)
    {
        if (i >= 0 && i < items.Count)
        {
            selectedItemIndex = i;
        }
        else
        {
            selectedItemIndex = -1; // indicates no item is selected
        }

        return selectedItemIndex;
    }

    // get selected item
    public int GetSelectedItemIndex()
    {
        return selectedItemIndex;
    }

    // Assign an item to be equipped
    public int EquipItem(int i)
    {
        // deactivate the current GameObject that
        // is equipped in the game world
        Item current = GetEquippedItem();
        if (current != null)
        {
            current.InGameObject.SetActive(false);
        }

        // activate the in game object that is to be
        // equipped
        if (i >= 0 && i < items.Count)
        {
            equippedItemIndex = i;
            current = GetEquippedItem();
            current.InGameObject.SetActive(true);
        }
        else
        {
            equippedItemIndex = -1; // invalid item index given
        }

        return equippedItemIndex;
    }

    // Return the index of the item that is currently equipped
    public int GetEquippedItemIndex()
    {
        return equippedItemIndex;
    }

    // Unequip item
    public void PutAwayItem()
    {
        // disable the in game object attached to
        // the currently equipped item
        Item i = GetSelectedItem();
        if (i != null)
        {
            i.InGameObject.SetActive(false);
        }
        equippedItemIndex = -1;
    }

    // Get the selected item in the inventory
    public Item GetSelectedItem()
    {
        if (selectedItemIndex >= 0)
        {
            return items[selectedItemIndex];
        } else
        {
            return null;
        }
    }

    // Get the equipped item in the inventory
    public Item GetEquippedItem()
    {
        if (equippedItemIndex >= 0)
        {
            return items[equippedItemIndex];
        } else
        {
            return null;
        }
    }

    // get the index of an item in the inventory
    public int GetItemIndex(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Equals(item))
            {
                return i;
            } 
        }

        return -1;
    }

    // initTests() is a function that was used for testing purposes.
    // It initializes the inventory with some test items
    public void StartingInventory()
    {
        // create initial items and set the item descriptions
        Item gun = new Item("Gun", "A GUN?!!!");
        Item gasCan = new Item("Gas Can", "Full can of gas");
        Item mazeBoard = new Item("Maze Board", "Wierd maze enclosed in glass");
        Item watermelon = new Item("Watermelon", "An actual watermelon");
        Item banana = new Item("Banana", "A ripe banana");
        Item grape = new Item("Grape", "A big juicy grape");
        mazeBoard.LongDescription = "I found this 3D maze... It looks like there is a button at the center of it. But how do I press it?";
        gun.LongDescription = "Can't believe I found a gun. I hope I won't have to use it.";
        gasCan.LongDescription = "Found a full gas can... What takes gas around here though?";
        watermelon.LongDescription = "I really hope somebody took the seeds out of this.";
        banana.LongDescription = "I prefer my bananas in bread form...";
        grape.LongDescription = "Seriously, why is there fruit in here?";

        // add the items to the inventory
        AddItem(gun);
        AddItem(gasCan);
        AddItem(mazeBoard);
        AddItem(watermelon);
        AddItem(banana);
        AddItem(grape);
    }
    #endregion
}

