using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour
{
    public bool isInvisible;

    // **********************
    // ** Brad added below **

    // setting isInvisible to false within Start() is done to avoid the "null object reference"
    // error if isInvisible was not set within the inspector window
    public void Start()
    {
        isInvisible = false;
    }

    // Update() is called once per frame
    public void Update()
    {
        // The following was added for testing purposes. Player can toggle their visibility
        // to the AI on and off by pressing "i"
        if (Input.GetKeyDown("i"))
        {
            isInvisible = !isInvisible;
            Debug.Log("Player Visible: " + !isInvisible);
        }
    }

    // ** Brad added above **
    // **********************
}
