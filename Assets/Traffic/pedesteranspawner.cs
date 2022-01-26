using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pedesteranspawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pedprefab;
    public int count;
    void Start()
    {
        StartCoroutine("spawn");
    }

    // Update is called once per frame
    IEnumerator spawn()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(pedprefab);
            Transform chiled = transform.GetChild(Random.Range(0, transform.childCount - 1));
            obj.GetComponent<WaypointNavagiator>().curruntwaypoint = chiled.GetComponent<waypoint>();
            obj.transform.position = chiled.position;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
