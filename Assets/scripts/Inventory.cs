using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // initialization of single inventory instance:
    public static Inventory inventoryInstance;

    private void Awake()
    {
        inventoryInstance = this;
    }

    // attributes:
    private List<Item> items = new List<Item>();

    public void Add(Item item)
    {
        items.Add(item);
    }

    public void Drop(Item item)
    {
        items.Remove(item);
    }
}
