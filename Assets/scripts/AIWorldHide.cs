using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton for identifying the Game Objects with the "Hide" tag
public sealed class AIWorldHide
{
    private static readonly AIWorldHide instance = new AIWorldHide();
    private static GameObject[] hidingSpots;

    static AIWorldHide()
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("Hide");
    }

    private AIWorldHide() { }

    public static AIWorldHide Instance
    {
        get { return instance; }
    }

    public GameObject[] GetHidingSpots()
    {
        return hidingSpots;
    }
}
