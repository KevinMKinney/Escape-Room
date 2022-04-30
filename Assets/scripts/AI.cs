using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

/// <summary>
/// AI class contains functions that implement different behavoirs for the AI.
/// Behavoirs include:
///   Traveling
///   Chasing
///   Wandering
///   Hiding
///   Seeing the Player
///   Touching the Player
///   Idenifying if the Player is looking at them
///   Identify is the Player is close to them
///   Patrolling
/// The AI uses these behavoirs to find and catch the player
/// </summary>
public class AI : MonoBehaviour // This was Auto Generated // 122 V
{
    GameObject player;
    UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start() // This was Auto Generated
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
    void Chase()
    {
        Vector3 targetDir;
        targetDir = player.transform.position - this.transform.position; // Direction of the Player from the AI

        float heading;
        heading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(player.transform.forward)); // Angle between forward directions of player and AI

        float toTarget;
        toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        // If the player is moving slow, or the AI is in front of the Player and turning around
        if ((player.GetComponent<FirstPersonController>().currentSpeed < 0.02f) || toTarget > 90 && heading < 20)
        {
            GoTo(player.transform.position);
            return;
        }

        float lookAhead;
        lookAhead = targetDir.magnitude / (agent.speed + player.GetComponent<FirstPersonController>().currentSpeed); // Predict the future location of the target
        GoTo(player.transform.position + player.transform.forward * lookAhead); // Travel to the predicted future location ofthe player
    }

    Vector3 wanderTarget = Vector3.zero;

    // Wander Around
    void Wander()
    {
        float wanderRadius;
        wanderRadius = 10;
        float wanderDistance;
        wanderDistance = 10;
        float wanderJitter;
        wanderJitter = 10;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter); // Location for the AI to travel

        wanderTarget.Normalize();
        wanderTarget *= wanderRadius; // Wander Target is on the circumference of the circle

        Vector3 targetLocal;
        targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance); // Distance the circle is in front of the AI
        Vector3 targetWorld;
        targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal); // Puts the Circle in the Game World, so the AI can travel to it

        GoTo(targetWorld); // Travel to the Target location
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
        chosenGO = AIWorldHide.Instance.GetHidingSpots()[0]; // Grab the Hiding Spots on the Map

        for (int i = 0; i < AIWorldHide.Instance.GetHidingSpots().Length; i++) // For every Hiding Spot
        {
            Vector3 hideDir;
            hideDir = AIWorldHide.Instance.GetHidingSpots()[i].transform.position - player.transform.position; // Direction to Hide from Player
            Vector3 hidePos;
            hidePos = AIWorldHide.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 4; // Where the AI should Hide

            if (Vector3.Distance(this.transform.position, hidePos) < dist) // Find the nearest Hiding Spot
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = AIWorldHide.Instance.GetHidingSpots()[i];
                dist = Vector3.Distance(this.transform.position, hidePos);
            }
        }

        // Shoot a Ray Back at the Hiding Spot to find a location, so the AI can get close to the Hiding Spot
        Collider hideCol;
        hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay;
        backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance;
        distance = 250.0f;
        hideCol.Raycast(backRay, out info, distance);

        GoTo(info.point + chosenDir.normalized * 3); // Travel to the location
    }

    // Returns True if there are no objects inbetween the AI and the Player, otherwise False
    bool CanSeePlayer()
    {
        RaycastHit raycastInfo;
        Vector3 rayToPlayer;
        rayToPlayer = player.transform.position - this.transform.position; // Direction of the Player from AI
        if (Physics.Raycast(this.transform.position, rayToPlayer, out raycastInfo)) // Shoot Ray in direction of the Player
        {
            if (raycastInfo.transform.gameObject.tag == "Player") // If Ray touched the Player
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
        toAgent = this.transform.position - player.transform.position; // Direction of the AI from Player
        float lookingAngle;
        lookingAngle = Vector3.Angle(player.transform.forward, toAgent); // Where the Player is facing

        if (lookingAngle < 62) // Agent is within an angle of 62 degrees of the front of the player
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

    // Returns True if the AI is within 30 meters of the Player, otherwise False
    bool PlayerInRange()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) < 30) // Player is within 30 meters of AI
        {
            return true;
        }
        return false;
    }

    // Returns True if the AI is within 2.5 meters of the Player, otherwise False
    bool IsTouchingPlayer()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) < 2.5f) // AI is within 2.5 meters of player
        {
            return true;
        }
        return false;
    }

    int currentWP = 0;

    // AI Travels to all the Waypoints on the Map
    void Patrol()
    {
        Vector3 chosenWP;
        chosenWP = AIWorldWP.Instance.GetWaypoints()[currentWP].transform.position; // Grab WP position

        if (Vector3.Distance(this.transform.position, chosenWP) < 4) // At current Waypoint
        {
            currentWP++;
        }
        if (currentWP >= AIWorldWP.Instance.GetWaypoints().Length) // Current Waypoint is greater than array of waypoints
        {
            currentWP = 0;
        }

        GoTo(chosenWP); // travel to WP
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    }


    //Section Writen By Nicole Batchelor

    bool PlayerIsInvisable()
    {
        return player.GetComponent<Invisible>().isInvisible;
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    }

    //End Code Written By Nicole Batchelor

    // Update is called once per frame
    void Update() // This was Auto Generated
    {
        if (IsTouchingPlayer()) // Too close to player
        {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            // End the Game / Quit Application
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
=======
            UnityEditor.EditorApplication.isPlaying = false; // End the Game
>>>>>>> Stashed changes
        }
        else if (!coolDown) // Not on cooldown
        {
<<<<<<< Updated upstream
            if (!PlayerInRange() || PlayerIsInvisable())
=======
            if (!PlayerInRange()) // Player not in range
>>>>>>> Stashed changes
=======
=======
>>>>>>> Stashed changes
            UnityEditor.EditorApplication.isPlaying = false; // End the Game
        }
        else if (!coolDown) // Not on cooldown
        {
            if (!PlayerInRange()) // Player not in range
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
            {
                Patrol();
            }
            /*else if (CanSeePlayer() && PlayerCanSeeMe())
            {
                // Work in Progress
                coolDown = true;
                Invoke("BehaviorCoolDown", 3);
            }*/
            else
            {
                Chase();
            }
        }
    }
}
