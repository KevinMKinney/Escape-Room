using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AINew is a implementation of how the AI can interact and perform its goals.
/// The AI uses a goal orientated action planner to execute its behavoir and tasks.
/// </summary>
public class AINew : AIGAgent // 8 V
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        SubGoal s1;
        s1 = new SubGoal("CaughtPlayer", 1, true); // Goal of catching the player
        goals.Add(s1, 3);
    }
}
