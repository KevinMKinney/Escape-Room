using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankHammer : MonoBehaviour
{
    // Start is called before the first frame update
    bool plankDown;
    public pick_up hammer;
    public raycast Raycast;
    void Clicked()
    {
        Raycast.canthrow = true;
        pick_up playerHolding = null;
        if (!plankDown)
        {
            foreach (pick_up itemObject in FindObjectsOfType<pick_up>())
            {
                if (itemObject.pickedup == 2)
                {
                    playerHolding = itemObject;
                }
            }
            if ((playerHolding != null) && (playerHolding == hammer.GetComponent<pick_up>()))
            {
                GetComponent<Animation>().Play();
                plankDown = true;
            }
        }
    }

   
}
