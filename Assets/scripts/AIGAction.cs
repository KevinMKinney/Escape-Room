using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIGAction : MonoBehaviour // 35 V
{
    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target;
    public string targetTag;
    public float duration = 0;
    public AIWorldState[] preConditions;
    public AIWorldState[] afterEffects;
    public NavMeshAgent agent;

    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    public AIWorldStates agentBeliefs;

    public AIGInventory inventory;

    public bool running = false;

    public AIGAction() // Dictionary of preconditions and effects for the world states
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    public void Awake() // When scipt is loaded
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        if(preConditions != null) // Add preconditions
        {
            foreach(AIWorldState i in preConditions)
            {
                preconditions.Add(i.key, i.value);
            }
        }
        if (afterEffects != null) // Add after effects
        {
            foreach (AIWorldState i in afterEffects)
            {
                effects.Add(i.key, i.value);
            }
        }

        inventory = this.GetComponent<AIGAgent>().inventory; // Grab inventory
    }

    public bool IsAchievable()
    {
        return true;
    }

    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach(KeyValuePair<string, int> i in preconditions) // For each precondition
        {
            if (!conditions.ContainsKey(i.Key)) // If it is not present, then plan is not achieveable
            {
                return false;
            }
        }
        return true;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
