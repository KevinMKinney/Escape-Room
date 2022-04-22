using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSphereGenerator : MonoBehaviour
{
    
    void Start()
    {
        GetComponent<MeshFilter>().sharedMesh = BlobGenerator.GenerateBlobMesh();
    }
}
