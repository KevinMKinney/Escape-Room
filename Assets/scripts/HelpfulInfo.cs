using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Written this semester.
 * Holds helpfull info using static propertys, for all scripts to use.
 */
public class HelpfulInfo : MonoBehaviour
{
    //Spawn Places for
    public static List<GameObject> SpawnPlaces;
    public List<GameObject> spawnPlaces;

    //When the game starts, set the non-static propertys to their static counter parts.
    void Awake()
    {
        SpawnPlaces = spawnPlaces; 
    }
}
