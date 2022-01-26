using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
public class lockdoor : MonoBehaviour
{

    public int doormode = 3;
    public GameObject key;
    public int wait = 1000;
    pick_up cow;
    public bool open = true;
    bool canopen = true;
    bool locked = true; 
    void Update()
    {
        cow = null;
        wait++;
        if (!locked)
        {
            if ((!open) && (canopen))
            {
                canopen = false;
                open = true;
                transform.parent.GetComponent<Animation>().Play("door2");
                StartCoroutine("cowy");
            }
        }
        else
        {
            if (!open)
            {
               
                if (key == null)
                {
                    locked = false;
                }
                else
                {
                    foreach (pick_up islock in FindObjectsOfType<pick_up>())
                    {
                        if (islock.pickedup == 2)
                        {
                            cow = islock;
                        }
                    }
                    if (cow == key.GetComponent<pick_up>())
                    {
                        locked = false;
                        canopen = false;
                      //  open = true;
                        transform.parent.GetComponent<Animation>().Play("door2");
                        StartCoroutine("cowy");


                    }
                    else
                    {
                        wait = 0;
                        FindObjectOfType<Text>().text = "I need a key";
                        StartCoroutine("cows");
                        doormode = 3;
                    }

                }
           }
        }
    }
    IEnumerator cowy()
    {
        yield return new WaitForSeconds(5);
        open = true;
        canopen = true;
    }
    IEnumerator cows()
    {
        yield return new WaitForSeconds(2);
        FindObjectOfType<Text>().text = "";
        open = true;
    }
}