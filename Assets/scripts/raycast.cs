using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycast : MonoBehaviour
{
    public bool canthrow = false;
    public GameObject Fog;
    bool FogBoll;
    
    /*private void OnTriggerEnter(Collider other)
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
    void Update()
    {
        canthrow = true;
        
        RaycastHit raycastHit;
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(transform.position, transform.GetComponent<Camera>().transform.TransformDirection(Vector3.forward), out raycastHit))
            {
                if ((raycastHit.collider.GetComponent<door>() != null) || (raycastHit.collider.GetComponent<ExitDoor>() != null))
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
                        
                        if ((raycastHit.collider.GetComponent<pick_up>() != null) && (raycastHit.distance < 5))
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
                                    canthrow = false;
                                    raycastHit.collider.SendMessage("Clicked", SendMessageOptions.DontRequireReceiver);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
