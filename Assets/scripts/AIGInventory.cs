using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AIG Inventory keeps track of the inventory of the world and has methods that can be used to add, remove, or find items in the inventory
/// </summary>
public class AIGInventory // 20 V
{
    List<GameObject> items = new List<GameObject>();

    public void AddItem(GameObject i) // Add GameObject to the items list 
    {
        items.Add(i);
    }

    public GameObject FindItemWithTag(string tag) // Find GameObject with certain tag
    {
        foreach(GameObject i in items) // If item tag is same as given tag return the GameObject item
        {
            if(i.tag == tag)
            {
                return i;
            }
        }
        return null;
    }

    public void RemoveItem(GameObject i) // Remove item from the inventory
    {
        int indexToRemove;
        indexToRemove = -1;

        foreach(GameObject g in items) // For each GameObject in the items list, if gameobjects match then remove it
        {
            indexToRemove++;
            if(g == i)
            {
                break;
            }
        }
        if(indexToRemove >= -1) // Remove item at index spot
        {
            items.RemoveAt(indexToRemove);
        }
    }
}
