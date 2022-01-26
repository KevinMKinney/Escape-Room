using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavagiator : MonoBehaviour
{
    // Start is called before the first frame update
    CharacterNavigationControllor controller;
    public waypoint curruntwaypoint;
    public int direction;
    private void Awake()
    {
        controller = GetComponent<CharacterNavigationControllor>();
    }
    void Start()
    {
        StartCoroutine("move");
        direction = Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (controller.reacheddestionation)
        {
            bool shouldbranch = false;
            if (curruntwaypoint.branches != null && curruntwaypoint.branches.Count > 0)
            {
                shouldbranch = Random.Range(0.0001f,1.001f) <= curruntwaypoint.branchRatio ? true : false;
            }
            if (shouldbranch)
            {
                curruntwaypoint = curruntwaypoint.branches[Random.Range(0, curruntwaypoint.branches.Count)];
            }
            else
            {
                if (direction == 1)
                {
                    if (curruntwaypoint.next != null)
                    {
                        curruntwaypoint = curruntwaypoint.next;
                    } else
                    {
                        direction = 0;
                        curruntwaypoint = curruntwaypoint.previous;
                    }
                }
                if (direction == 0)
                {
                    if (curruntwaypoint.previous != null)
                    {
                        curruntwaypoint = curruntwaypoint.previous;
                    }
                    else
                    {
                        direction = 1;
                        curruntwaypoint = curruntwaypoint.next;
                    }
                }
            }
            controller.SetDestionation(curruntwaypoint.Getposition());
        }
    }
    IEnumerator move ()
    {
        yield return new WaitForSeconds(0.1f);
        controller.SetDestionation(curruntwaypoint.Getposition());
    }
}
