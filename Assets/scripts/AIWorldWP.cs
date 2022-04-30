using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton for identifying all the Game Objects with the "Waypoint" tag.
/// Used by the AI to patrol around the world
/// </summary>
public sealed class AIWorldWP // 13 V
{
    private static readonly AIWorldWP instance = new AIWorldWP();
    private static GameObject[] waypoints; // Array of Waypoints

    // Fill the array with all the game objects that have a "Waypoint" tag
    static AIWorldWP()
    {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    }

    private AIWorldWP() { }


    // Return the readonly instance
    public static AIWorldWP Instance
    {
        get { 
            return instance; 
        }
    }

    // Return the array of Waypoints
    public GameObject[] GetWaypoints()
    {
        return waypoints;
    }
}