using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AIGGoToPlayer class is an action that is available for the AI to use.
/// There are no pre or post conditions or effects on the State of the world
/// </summary>
public class AIGoToPlayer : AIGAction // 7 V
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        return true;
    }
}
