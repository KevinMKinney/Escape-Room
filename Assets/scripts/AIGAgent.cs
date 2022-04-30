using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityStandardAssets.Characters.FirstPerson;

/// <summary>
/// SubGoal class adds a new goal for the AI to achieve
/// </summary>
public class SubGoal // 72 V
{
    public Dictionary<string, int> sgoals;
    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        sgoals = new Dictionary<string, int>();
        sgoals.Add(s, i); // Add subgoal to the sgoals dictioanry
        remove = r;
    }
}


/// <summary>
/// AIGAgent class takes all the actions, goals, and inventory of the world to control the behavoir of the AI
/// The class makes sure the goal has not yet been completed and there are still action waiting to be executed
/// If there is an action that needs to still be completed then the class will instruct the AI to behave in a way suited to complete the action.
/// </summary>
public class AIGAgent : MonoBehaviour
{
    public List<AIGAction> actions = new List<AIGAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public AIGInventory inventory = new AIGInventory();

    AIGPlanner planner;
    Queue<AIGAction> actionQueue;
    public AIGAction currentAction;
    SubGoal currentGoal;

    float timer = 0.0f;
    Vector3 wanderTarget = Vector3.zero;

    // Start is called before the first frame update
    public void Start()
    {
        AIGAction[] acts;
        acts = this.GetComponents<AIGAction>();

        foreach(AIGAction i in acts) // Add each action to the actions list
        {
            actions.Add(i);
        }
    }

    void GoTo(Vector3 location)
    {
        currentAction.agent.SetDestination(location);
    }

    bool invoked = false;

    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    void LateUpdate()
    {
        if(currentAction != null && currentAction.running) // and current action is ...
        {
            if(currentAction.actionName == "AI Go To Player")
            {
                if (currentAction.agent.hasPath && currentAction.agent.remainingDistance < 2.5f) // Complete Goal
                {
                    if (!invoked) // Complete action
                    {
                        Invoke("CompleteAction", currentAction.duration);
                        invoked = true;
                    }
                }

                Vector3 targetDir;
                targetDir = currentAction.target.transform.position - currentAction.agent.transform.position; // Direction of the Player from the AI

                float heading;
                heading = Vector3.Angle(currentAction.agent.transform.forward, currentAction.agent.transform.TransformVector(currentAction.target.transform.forward)); // Angle between forward directions of player and AI

                float toTarget;
                toTarget = Vector3.Angle(currentAction.agent.transform.forward, currentAction.agent.transform.TransformVector(targetDir));

                // If the player is moving slow, or the AI is in front of the Player and turning around
                if ((currentAction.target.GetComponent<FirstPersonController>().currentSpeed < 0.02f) || toTarget > 90 && heading < 20)
                {
                    GoTo(currentAction.target.transform.position); //currentAction.agent.SetDestination(currentAction.target.transform.position);
                    return;
                }

                float lookAhead;
                lookAhead = targetDir.magnitude / (currentAction.agent.speed + currentAction.target.GetComponent<FirstPersonController>().currentSpeed); // Predict the future location of the target
                GoTo(currentAction.target.transform.position + currentAction.target.transform.forward * lookAhead); // Travel to the predicted future location ofthe player
                return;
            }

            else if(currentAction.actionName == "AI Patrol Item")
            {
                if (currentAction.agent.hasPath && currentAction.agent.remainingDistance < 2.5f) // Complete Goal
                {
                    if (!invoked) // Complete action
                    {
                        Invoke("CompleteAction", currentAction.duration);
                        invoked = true;
                    }
                }

                GoTo(currentAction.target.transform.position);
            }
            else if(currentAction.actionName == "Wander")
            {
                float waitTime = 3.0f;
                timer += Time.deltaTime;

                if (currentAction.agent.hasPath && (timer > waitTime)) // Complete Goal
                {
                    if (!invoked) // Complete action
                    {
                        Invoke("CompleteAction", currentAction.duration);
                        invoked = true;
                    }
                }
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
                targetWorld = currentAction.agent.transform.InverseTransformVector(targetLocal); // Puts the Circle in the Game World, so the AI can travel to it

                GoTo(targetWorld); // Travel to the Target location
            }
        }

        if(planner == null || actionQueue == null) // If nothing is ready to be executed
        {
            planner = new AIGPlanner();
            var sortedGoals = from entry in goals orderby entry.Value descending select entry; // Sort the goals the AI has to complete w.r.p the value of the goal

            foreach(KeyValuePair<SubGoal, int> sg in sortedGoals) // For each goal add the actions to the queue
            {
                actionQueue = planner.plan(actions, sg.Key.sgoals, null);
                if(actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        if(actionQueue != null && actionQueue.Count == 0) // If there are no more actions in the queue
        {
            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }

            planner = null;
        }

        if(actionQueue != null && actionQueue.Count > 0) // If there are more actions waiting to be executed
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                if(currentAction.target == null && currentAction.targetTag != "")
                {
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag); // Target of the action has the target tag
                }
                if(currentAction.target != null)
                {
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position); // Target of the action is the GameObject
                }
            }
            else
            {
                actionQueue = null;
            }
        }
    }
}
