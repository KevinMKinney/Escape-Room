using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ExitDoor : MonoBehaviour
{
    // Start is called before the first frame update
    public int doremode = 3;
    bool open;
    public GameObject key;
    public int wait = 1000;
    public List<GameObject> locks;
    pick_up heldObject;
    // Update is called once per frame
    void Update()
    {
        heldObject = null;
        wait++;
       
        if (doremode == 1)
        {
            doremode = 3;
            wait = 0;
            if (key == null)
            {
                SceneManager.LoadScene("EXIT");
            }
            else
            {
                foreach (pick_up objectInQuestion in FindObjectsOfType<pick_up>())
                {
                    if (objectInQuestion.pickedup == 2)
                    {
                        heldObject = objectInQuestion;
                    }
                }
                if (heldObject == key.GetComponent<pick_up>())
                {
                    bool flag = false;
                    foreach (GameObject lok in locks)
                    {
                        if (lok != null)
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        SceneManager.LoadScene("TRansitionToOtherScenesLikeEXIT");
                    }
                    else
                    {
                        wait = 0;
                        FindObjectOfType<Text>().text = "I can not open the door yet";
                        StartCoroutine("RemoveWord");
                        doremode = 3;
                    }
                }
                else
                {
                    wait = 0;
                    FindObjectOfType<Text>().text = "I need a house key";
                    StartCoroutine("RemoveWord");
                    doremode = 3;
                }

            }
        }
    }
    IEnumerator RemoveWord()
    {
        yield return new WaitForSeconds(2);
        FindObjectOfType<Text>().text = "";
        doremode = 3;
    }
}
