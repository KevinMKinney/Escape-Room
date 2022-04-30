using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AISpawn class generates a specified number of AI at the start of the game.
/// </summary>
public class AISpawn : MonoBehaviour // 8 V
{
    public GameObject AIPrefab; // Grab the AI prefab
    public int numAI;

    // Start is called before the first frame update
    void Start()
    {
        int i;
        for(i = 0; i < numAI; i++) // Spawn ai into the world
        {
            Instantiate(AIPrefab, this.transform.position, Quaternion.identity);
        }
    }
}
