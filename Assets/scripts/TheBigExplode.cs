using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBigExplode : MonoBehaviour
{
    public GameObject explosionPrefab;
    // Start is called before the first frame update
    public void TargitHit(float  _)
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        transform.position = new Vector3(0,-999,0);
    }
}
