using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour
{
    public bool isInvisible;


    // Brad added the following constructor to avoid the "null object reference" error if
    // isInvisible was not set within the inspector window
    public void Start()
    {
        isInvisible = false;
    }

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
}
