using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that keeps track of the state of the world.
/// Can add and remove items from the inventory of the world that allows the AI to keep track of what the player has.
/// </summary>
public sealed class AIGWorld // 28 V
{
    private static readonly AIGWorld instance = new AIGWorld();
    private static AIWorldStates world;
    private static Queue<GameObject> items;

    static AIGWorld()
    {
        world = new AIWorldStates();
        items = new Queue<GameObject>();

        GameObject[] item;
        item = GameObject.FindGameObjectsWithTag("Item"); // Will need to set the tags of all the items in the editor

        foreach(GameObject c in item) // Enqueue the item in the items list
        {
            items.Enqueue(c);
        }

        if(item.Length > 0) // Modify the world state to the number of items available to patrol
        {
            world.ModifyState("ItemsToPatrol", item.Length);
        }
    }

    private AIGWorld() { }

    public void AddItem(GameObject p) // Add GameObject to the items list
    {
        items.Enqueue(p);
    }

    public GameObject RemoveItem() // Remove and return the GameObject from the items list
    {
        if (items.Count == 0)
        {
            return null;
        }
        return items.Dequeue();
    }

    // Return the readonly instance
    public static AIGWorld Instance
    {
        get { 
            return instance; 
        }
    }

    // Return the state of the game
    public AIWorldStates GetWorld()
    {
        return world;
    }
}
