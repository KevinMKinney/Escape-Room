using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Written this semester.
 * Holds helpfull info using static properties, for all scripts to use.
 */
public class HelpfulInfo : MonoBehaviour
{
    public static List<GameObject> SpawnPlaces;
    public List<GameObject> spawnPlaces;

    //When the game starts, set the non-static properties to their static counter parts.
    void Awake()
    {
        SpawnPlaces = spawnPlaces; 
    }
}
