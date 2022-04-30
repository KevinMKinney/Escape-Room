using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AIPatrolItem is an action that the AI is able to use.
/// There must be an item available in the world as the prerequiste to this action.
/// </summary>
public class AIPatrolItem : AIGAction // 14 V
{
    GameObject resource;
    public override bool PrePerform()
    {
        resource = AIGWorld.Instance.RemoveItem(); // Grab item from the world
        if(resource != null)
        {
            inventory.AddItem(resource); // Add item to the world's inventory
        }
        else
        {
            return false;
        }

        AIGWorld.Instance.GetWorld().ModifyState("ItemsToPatrol", -1); // Remove one item to patrol from the world state
        return true;
    }

    public override bool PostPerform()
    {
        AIGWorld.Instance.GetWorld().AddState("AtItem", 1);
        return true;
    }
}
