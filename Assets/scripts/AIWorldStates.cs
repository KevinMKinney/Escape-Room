using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Each World State can have a key and value which is used to identify what is happening in the environment
/// </summary>
[System.Serializable]
public class AIWorldState // 32 V
{
    public string key;
    public int value;
}

/// <summary>
/// Creates a Dictionary to keep track of World States.
/// AIWorldStates class can add, remove, or modify the state of the world which is useful for the action planner
/// </summary>
public class AIWorldStates
{
    public Dictionary<string, int> states;

    public AIWorldStates()
    {
        states = new Dictionary<string, int>();
    }

    public bool HasState(string key) // Check if the states dictionary has a state
    {
        return states.ContainsKey(key);
    }

    public void AddState(string key, int value) // Add a state to the dictionary of states
    {
        states.Add(key, value);
    }

    public void ModifyState(string key, int value) // Modify a State
    {
        if (states.ContainsKey(key)) // If the state is present
        {
            states[key] += value; // Add the value to the states key and remove if less than 0
            if (states[key] <= 0)
            {
                RemoveState(key);
            }
        }
        else // Otherwise add
        {
            states.Add(key, value);
        }
    }

    public void RemoveState(string key) // Remove a State
    {
        if (states.ContainsKey(key))
        {
            states.Remove(key);
        }
    }

    public void SetState(string key, int value) // Set a State to a certain value
    {
        if (states.ContainsKey(key))
        {
            states[key] = value;
        }
        else
        {
            states.Add(key, value);
        }
    }

    public Dictionary<string, int> GetStates() // Return all of the world states
    {
        return states;
    }
}
