using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Node class sets up the fields of each node that will be used for the action planner
/// </summary>
public class Node  // 90 V
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public AIGAction action;

    public Node(Node parent, float cost, Dictionary<string, int> allstates, AIGAction action) // Structure of each node
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates);
        this.action = action;
    }
}

/// <summary>
/// AIGPlanner class cycles through available actions and builds all possible bridges connecting the goal to the current state of the world.
/// If there are multiple available paths to be choosen from, then the planner will find the path with the smallest cost.
/// After the path with the smallest cost is chosent then it will enqueue those actions for the AI to execute.
/// </summary>
public class AIGPlanner
{
    public Queue<AIGAction> plan(List<AIGAction> actions, Dictionary<string, int> goal, AIWorldStates states)
    {
        List<AIGAction> usableActions;
        usableActions = new List<AIGAction>();

        foreach(AIGAction a in actions) // For each action if it is acievable then add it to the usable actions list
        {
            if (a.IsAchievable())
            {
                usableActions.Add(a);
            }
        }

        List<Node> leaves;
        leaves = new List<Node>();
        Node start;
        start = new Node(null, 0, AIGWorld.Instance.GetWorld().GetStates(), null); // Start node is the current state of the world

        bool success;
        success = AIBuildGraph(start, leaves, usableActions, goal);

        if (!success) // If a plan was not able to be formed
        {
            Debug.Log("NO PLAN");
            return null;
        }

        Node cheapest;
        cheapest = null;

        foreach(Node leaf in leaves) // For each node in the plan combine the cheapest nodes
        {
            if(cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if(leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        List<AIGAction> result;
        result = new List<AIGAction>();
        Node c;
        c = cheapest;

        while(c != null) // Insert the cheapest node into the final action list
        {
            if(c.action != null)
            {
                result.Insert(0, c.action);
            }
            c = c.parent;
        }

        Queue<AIGAction> queue;
        queue = new Queue<AIGAction>();

        foreach(AIGAction r in result) // Queue each action
        {
            queue.Enqueue(r);
        }

        Debug.Log("The Plan is: ");
        foreach(AIGAction q in queue) // Display the actions that will occur in the editor log
        {
            Debug.Log("Q: " + q.actionName);
        }

        return queue;
    }

    private bool AIBuildGraph(Node parent, List<Node> leaves, List<AIGAction> usuableActions, Dictionary<string, int> goal)
    {
        bool foundPath;
        foundPath = false;

        foreach(AIGAction action in usuableActions) // For each usable action
        {
            if (action.IsAchievableGiven(parent.state)) // If it can be achieved given the parents world state
            {
                Dictionary<string, int> currentState;
                currentState = new Dictionary<string, int>(parent.state);

                foreach(KeyValuePair<string, int> i in action.effects) // For each effect of the action add it to the current state if not already
                {
                    if (!currentState.ContainsKey(i.Key))
                    {
                        currentState.Add(i.Key, i.Value);
                    }
                }

                Node node;
                node = new Node(parent, parent.cost + action.cost, currentState, action);

                if(GoalAchieved(goal, currentState)) // If goal can be achieved path is found and add the node to the list
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else // Try forming Plan with subset of actions
                {
                    List<AIGAction> subset;
                    subset = ActionSubset(usuableActions, action);
                    bool found;
                    found = AIBuildGraph(node, leaves, subset, goal);

                    if (found) // If successful a path has been found
                    {
                        foundPath = true;
                    }
                }
            }
        }

        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string, int> i in goal) // For each pair in the dictionary if the state dictionary does not contain it then it will return false
        {
            if (!state.ContainsKey(i.Key))
            {
                return false;
            }
        }
        return true;
    }

    private List<AIGAction> ActionSubset(List<AIGAction> actions, AIGAction removeMe)
    {
        List<AIGAction> subset;
        subset = new List<AIGAction>();

        foreach(AIGAction i in actions) // For each action in the action list
        {
            if (!i.Equals(removeMe)) // If it is not equal to the passed action, then it will be added to the subset action list
            {
                subset.Add(i);
            }
        }
        return subset;
    }
}
