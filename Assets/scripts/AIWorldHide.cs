using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton for identifying the Game Objects with the "Hide" tag.
/// Used by the AI to find hiding spots in the environment
/// </summary>
public sealed class AIWorldHide // 13 V
{
    private static readonly AIWorldHide instance = new AIWorldHide();
    private static GameObject[] hidingSpots; // Array of Hiding Spots

    // Fill the array with all of the game objects that have the "Hide" tag
    static AIWorldHide()
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("Hide");
    }

    private AIWorldHide() { }

    // Return the readonly instance
    public static AIWorldHide Instance
    {
        get { 
            return instance; 
        }
    }

    // Return the array of HidingSpots
    public GameObject[] GetHidingSpots()
    {
        return hidingSpots;
    }
}
