using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypoint : MonoBehaviour
{
    // Start is called before the first frame update
    public waypoint next;
    public waypoint previous;
   // public GameObject subtree;
   // public int chance = 50;
    [Range(0f, 5f)]
    public float width = 1f;
    public List<waypoint> branches;
    [Range(0.1f, 1f)]
    public float branchRatio = 0.5f;
    public Vector3 Getposition() {
        Vector3 minBound = transform.position + transform.right * width / 2f;
        Vector3 MaxBound = transform.position - transform.right * width / 2f;

        return Vector3.Lerp(minBound, MaxBound, Random.Range(0f, 1f));
    }
}
