using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Written this semester.
 * This script allows a GameObject to explode when shot by the gun. 
 * When it 'Explodes' the object is placed in the void, where the pick_up script places it randomly in the house on the next frame.
 */
public class TheBigExplode : MonoBehaviour
{
    public GameObject explosionPrefab;
    
    public void TargitHit(float  _)
    {
        // Create a copy of the explosion and place it at the objects location. The explosion is now active and will be seen.
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        //Place current object in the void.
        transform.position = new Vector3(0,-999,0);
    }
}
