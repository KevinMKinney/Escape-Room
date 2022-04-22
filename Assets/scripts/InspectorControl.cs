using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorControl : MonoBehaviour
{
    Item inspectedItem;

    // Start is called before the first frame update
    void Start()
    {
        inspectedItem = this.GetComponent<Item>();

        // disable this script for now...
        this.GetComponent<InspectorControl>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            Debug.Log("Up");
        } 
        
        if (Input.GetKeyDown("right"))
        {
            Debug.Log("Right");
        }

        if (Input.GetKeyDown("down"))
        {
            Debug.Log("Down");
        }

        if (Input.GetKeyDown("left"))
        {
            Debug.Log("Left");
        }
    }
}
