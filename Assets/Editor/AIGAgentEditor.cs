using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AIGAgentVisualEditor class allows developers to see what the AI is planning to do through actions.
/// This adds the current action being executed, along with all the action available to the AI.
/// At the bottom the current goal of the AI is displayed in the Unity editor
/// </summary>
[CustomEditor(typeof(AIGAgentVisual))]
[CanEditMultipleObjects]
public class AIGAgentVisualEditor : Editor // 28 V
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
        AIGAgentVisual agent;
        agent = (AIGAgentVisual)target;

        GUILayout.Label("Current Action: " + agent.gameObject.GetComponent<AIGAgent>().currentAction);
        GUILayout.Label("All Actions: ");

        foreach (AIGAction a in agent.gameObject.GetComponent<AIGAgent>().actions)
        {
            string pre;
            pre = "";
            string eff;
            eff = "";

            foreach (KeyValuePair<string, int> p in a.preconditions)
            {
                pre += p.Key + ", ";
            }
            foreach (KeyValuePair<string, int> e in a.effects)
            {
                eff += e.Key + ", ";
            }

            GUILayout.Label("  " + a.actionName + "(" + pre + ")(" + eff + ")");
        }

        GUILayout.Label("Goals: ");

        foreach (KeyValuePair<SubGoal, int> g in agent.gameObject.GetComponent<AIGAgent>().goals)
        {
            foreach (KeyValuePair<string, int> sg in g.Key.sgoals)
                GUILayout.Label("  " + sg.Key);
        }

        serializedObject.ApplyModifiedProperties();
    }
}