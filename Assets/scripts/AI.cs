using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

// AI Behavior
public class AI : MonoBehaviour
{
    GameObject player;
    UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        // Identify Player and AI
        player = GameObject.FindWithTag("Player");
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Travel to 3D Vector Location
    void GoTo(Vector3 location)
    {
        agent.SetDestination(location);
    }

    // Predict the Location of Player and Travel there
    void Pursue()
    {
        Vector3 targetDir;
        targetDir = player.transform.position - this.transform.position;

        float heading;
        heading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(player.transform.forward));

        float toTarget;
        toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        if((player.GetComponent<FirstPersonController>().currentSpeed < 0.02f) || toTarget > 90 && heading < 20)
        {
            GoTo(player.transform.position);
            return;
        }

        float lookAhead;
        lookAhead = targetDir.magnitude / (agent.speed + player.GetComponent<FirstPersonController>().currentSpeed);
        GoTo(player.transform.position + player.transform.forward * lookAhead);
    }

    Vector3 wanderTarget = Vector3.zero;

    // Wander Around
    void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 10;
        float wanderJitter = 10;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter);

        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal;
        targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld;
        targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        GoTo(targetWorld);
    }


    // Hide from the Player
    void GoHide()
    {
        float dist;
        dist = Mathf.Infinity;
        Vector3 chosenSpot;
        chosenSpot = Vector3.zero;
        Vector3 chosenDir;
        chosenDir = Vector3.zero;
        GameObject chosenGO;
        chosenGO = AIWorldHide.Instance.GetHidingSpots()[0];

        for (int i = 0; i < AIWorldHide.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir;
            hideDir = AIWorldHide.Instance.GetHidingSpots()[i].transform.position - player.transform.position;
            Vector3 hidePos;
            hidePos = AIWorldHide.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 4;

            if (Vector3.Distance(this.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = AIWorldHide.Instance.GetHidingSpots()[i];
                dist = Vector3.Distance(this.transform.position, hidePos);
            }
        }

        Collider hideCol;
        hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay;
        backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 250.0f;
        hideCol.Raycast(backRay, out info, distance);

        GoTo(info.point + chosenDir.normalized * 3);
    }

    // Returns True if there are no objects inbetween the AI and the Player, otherwise False
    bool CanSeePlayer()
    {
        RaycastHit raycastInfo;
        Vector3 rayToPlayer;
        rayToPlayer = player.transform.position - this.transform.position;
        if(Physics.Raycast(this.transform.position, rayToPlayer, out raycastInfo))
        {
            if(raycastInfo.transform.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    // Returns True if the Player is looking in the direction of the AI, otherwise False
    bool PlayerCanSeeMe()
    {
        Vector3 toAgent;
        toAgent = this.transform.position - player.transform.position;
        float lookingAngle;
        lookingAngle = Vector3.Angle(player.transform.forward, toAgent);

        if(lookingAngle < 62)
        {
            return true;
        }
        return false;
    }

    bool coolDown = false;
    
    // Reset Cooldown to False
    void BehaviorCoolDown()
    {
        coolDown = false;
    }

    // Returns True if the AI is within 20 meters of the Player, otherwise False
    bool PlayerInRange()
    {
        if(Vector3.Distance(this.transform.position, player.transform.position) < 30)
        {
            return true;
        }
        return false;
    }

    // Returns True if the AI is within 2.5 meters of the Player, otherwise False
    bool IsTouchingPlayer()
    {
        if(Vector3.Distance(this.transform.position, player.transform.position) < 2.5f)
        {
            return true;
        }
        return false;
    }

    int currentWP = 0;

    // AI Travels to all the Waypoints on the Map
    void Patrol()
    {
        Vector3 chosenWP = AIWorldWP.Instance.GetWaypoints()[currentWP].transform.position;

        if(Vector3.Distance(this.transform.position, chosenWP) < 4)
        {
            currentWP++;
        }
        if(currentWP >= AIWorldWP.Instance.GetWaypoints().Length)
        {
            currentWP = 0;
        }

        GoTo(chosenWP);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTouchingPlayer())
        {
            // End the Game / Quit Application
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else if (!coolDown)
        {
            if (!PlayerInRange())
            {
                Patrol();
            }
            /*else if (CanSeePlayer() && PlayerCanSeeMe())
            {
                GoHide();
                coolDown = true;
                Invoke("BehaviorCoolDown", 3);
            }*/
            else
            {
                Pursue();
            }
        }
    }
}
