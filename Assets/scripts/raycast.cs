using UnityEngine;

/* 
 * Lines 46 - 49, 96 - 136 and 145 - 163 written this semester.
 * Finds objects under the crosshair
 */
public class raycast : MonoBehaviour
{
    public bool canthrow = false;
    public GameObject Fog;

    UnityEngine.UI.Text text;

    /*
    bool FogBoll;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<goastPickup>() != null && other.GetComponent<goastPickup>().anger > 9.8f)
        {
            FogBoll = true;
            Fog.GetComponent<Animation>().Play("FogIn");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Goast") && FogBoll == true)
        {
            FogBoll = false;
            Fog.GetComponent<Animation>().Play("FogOut");
        }
    }*/
    private void Start()
    {
        text = FindObjectOfType<UnityEngine.UI.Text>();
    }
    void Update()
    {
        canthrow = true;

        RaycastHit raycastHit;
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(transform.position, transform.GetComponent<Camera>().transform.TransformDirection(Vector3.forward), out raycastHit))
            {
                //If the player is looking at the gas can but is pouring gas continue to pour gas.
                if(raycastHit.collider.GetComponent<GasCan>() != null && raycastHit.collider.GetComponent<GasCan>().frameSinceLastPour < 2)
                {
                    raycastHit.collider.GetComponent<GasCan>().ContinuePouringGas();
                } else if ((raycastHit.collider.GetComponent<door>() != null) || (raycastHit.collider.GetComponent<ExitDoor>() != null))
                {
                    canthrow = true;
                }
                if (raycastHit.transform.GetComponent<door>() != null)
                {
                    if (raycastHit.transform.GetComponent<door>().open)
                    {
                        raycastHit.transform.GetComponent<door>().open = false;
                    }
                }
                else
                {
                    if (raycastHit.transform.GetComponent<lockdoor>() != null)
                    {
                        if (raycastHit.transform.GetComponent<lockdoor>().open)
                        {
                            raycastHit.transform.GetComponent<lockdoor>().open = false;
                        }
                    }
                    else
                    {

                        if ((raycastHit.collider.GetComponent<pick_up>() != null) && (raycastHit.distance < 10))
                        {

                            if (raycastHit.collider.GetComponent<pick_up>().pickedup == 5)
                            {
                                raycastHit.collider.GetComponent<pick_up>().pickedup = 0;
                            }
                        }
                        else
                        {
                            if ((raycastHit.collider.GetComponent<Answer>() != null) && (raycastHit.distance < raycastHit.collider.GetComponent<Answer>().close))
                            {
                                raycastHit.collider.GetComponent<Answer>().CheckAnswer();

                            }
                            else
                            {

                                if ((raycastHit.collider.GetComponent<ExitDoor>() != null) && (raycastHit.distance < 9))
                                {
                                    raycastHit.collider.GetComponent<ExitDoor>().doremode = 1;
                                }
                                else
                                {
                                    //If a GameObject with the Starter script is under the crosshair than call it's clicked function. 
                                    if ((raycastHit.collider.GetComponent<Starter>() != null))
                                    {
                                        raycastHit.collider.GetComponent<Starter>().Clicked();
                                    }//If a GameObject with the OpenGasCap script is under the crosshair than call it's clicked function. 
                                    else if ((raycastHit.collider.GetComponent<OpenGasCap>() != null))
                                    {
                                        //If the gas cap is not open try to open it.
                                        if (!raycastHit.collider.GetComponent<OpenGasCap>().isOpen)
                                        {
                                            raycastHit.collider.GetComponent<OpenGasCap>().Clicked();
                                        }
                                        else
                                        {
                                            pick_up gasCan = null;
                                            //Loop though all items that are picked up and find the one being held.
                                            foreach (pick_up islock in FindObjectsOfType<pick_up>())
                                            {
                                                //The item is held.
                                                if (islock.pickedup == 2)
                                                {
                                                    //Save the item.
                                                    gasCan = islock;
                                                }
                                            }
                                            //If the player is holding the gas can and the gas cap is open then start to pour the gas.
                                            if(gasCan && gasCan.gameObject.name == "Gas Can")
                                            {
                                                gasCan.gameObject.GetComponent<GasCan>().ContinuePouringGas();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //If there is no clickable object under the crosshair than clear the text element.
                                        text.text = "";
                                        canthrow = false;
                                        //One last chance to have the Clicked function called! Calls Clicked on the GameObject if it has that function on it.
                                        raycastHit.collider.SendMessage("Clicked", SendMessageOptions.DontRequireReceiver);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            //If the player is not holding down the left mouse button than fire out a raycast anyway.
            if (Physics.Raycast(transform.position, transform.GetComponent<Camera>().transform.TransformDirection(Vector3.forward), out raycastHit))
            {
                // If the item can be picked up than display its name on the screen.
                if (raycastHit.transform.GetComponent<pick_up>() != null)
                {
                    // Make sure the text is not transparent.
                    Color color = text.color;
                    color.a = 1;
                    text.color = color;
                    // Set the text property to the name of the looked at object.
                    text.text = raycastHit.transform.GetComponent<pick_up>().name;
                }
                else
                {
                    // If there is no object under the crosshair than remove any text on screen.
                    text.text = "";
                }
            }
        }
    }
}
