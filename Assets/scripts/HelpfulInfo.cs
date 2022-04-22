using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpfulInfo : MonoBehaviour
{
    public static List<GameObject> SpawnPlaces;
    public List<GameObject> spawnPlaces;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlaces = spawnPlaces; 
    }
    
}
