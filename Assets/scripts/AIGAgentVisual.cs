using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AIGAgentVisual : MonoBehaviour // 5 V
{
    public AIGAgent thisAgent;

    // Start is called before the first frame update
    void Start()
    {
        thisAgent = this.GetComponent<AIGAgent>(); // Get the component of the AIGAgent
    }
}
