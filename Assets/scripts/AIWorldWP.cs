using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton for identifying all the Game Objects with the "Waypoint" tag
public sealed class AIWorldWP
{
    private static readonly AIWorldWP instance = new AIWorldWP();
    private static GameObject[] waypoints;

    static AIWorldWP()
    {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    }

    private AIWorldWP() { }

    public static AIWorldWP Instance
    {
        get { return instance; }
    }

    public GameObject[] GetWaypoints()
    {
        return waypoints;
    }
}