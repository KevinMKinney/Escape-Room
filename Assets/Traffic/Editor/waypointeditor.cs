using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WayPointEditor : EditorWindow
{
    [MenuItem("Window/Waypoint Creator")]
    public static void open()
    {
        GetWindow<WayPointEditor>();
    }
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(WayPointEditor));
    }
    public Transform waypointRoot;
    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);
        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));
        //if(EditorGUILayout)
        if (waypointRoot == null)
        {
            if (Random.Range(0, 10) == 1)
            {
                EditorGUILayout.HelpBox("Waypoint Root is Needed", MessageType.Error);
            }
            else
            {
                EditorGUILayout.HelpBox("Waypoint Root is Mandatory", MessageType.Error);
            }
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }
        obj.ApplyModifiedProperties();
    }
    void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
            CreateWaypoint();
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<waypoint>())
        {
            if (GUILayout.Button("Create waypoint Before"))
            {
                CreateWaypointBefore();
            }
            if (GUILayout.Button("Create waypoint After"))
            {
                CreateWaypointAfter();
            }
            if (GUILayout.Button("Remove waypoint"))
            {
                RemoveWaypoint();
            }
            if (GUILayout.Button("Add Branch Waypoint"))
            {
                CreateBranch();
            }
        }

    }

    void CreateWaypoint()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        waypoint Waypoint = waypointObject.GetComponent<waypoint>();
        if (waypointRoot.childCount > 1)
        {
            Waypoint.previous = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<waypoint>();
            Waypoint.previous.next = Waypoint;

            Waypoint.transform.position = Waypoint.previous.transform.position;
            Waypoint.transform.forward = Waypoint.previous.transform.forward;
        }
        Selection.activeGameObject = Waypoint.gameObject;
    }

    void CreateWaypointBefore()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        waypoint Waypoint = waypointObject.GetComponent<waypoint>();

        waypoint selectidwaypoint = Selection.activeGameObject.GetComponent<waypoint>();
        waypointObject.transform.position = selectidwaypoint.transform.position;
        waypointObject.transform.rotation = selectidwaypoint.transform.rotation;
        if (selectidwaypoint.previous != null)
        {
            Waypoint.previous = selectidwaypoint.previous;
            selectidwaypoint.previous.next = Waypoint;
        }
        Waypoint.next = selectidwaypoint;
        Selection.activeGameObject = Waypoint.gameObject;
        selectidwaypoint.previous = Waypoint;
        Waypoint.transform.SetSiblingIndex(selectidwaypoint.transform.GetSiblingIndex());
    }
    void CreateWaypointAfter()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        waypoint Waypoint = waypointObject.GetComponent<waypoint>();

        waypoint selectidwaypoint = Selection.activeGameObject.GetComponent<waypoint>();

        Waypoint.transform.position = selectidwaypoint.transform.position;
        Waypoint.transform.forward = selectidwaypoint.transform.forward;

        Waypoint.previous = selectidwaypoint;
        if (selectidwaypoint.next != null)
        {
            Waypoint.next = selectidwaypoint.next;
            selectidwaypoint.next.previous = Waypoint;
        }
        //Waypoint.next = selectidwaypoint;
        Selection.activeGameObject = Waypoint.gameObject;
        selectidwaypoint.next = Waypoint;
        Waypoint.transform.SetSiblingIndex(selectidwaypoint.transform.GetSiblingIndex());
    }
    void RemoveWaypoint()
    {
        waypoint selectidwaypoint = Selection.activeGameObject.GetComponent<waypoint>();
        if (selectidwaypoint.next != null)
        {
            selectidwaypoint.next.previous = selectidwaypoint.previous;
        }
        if (selectidwaypoint.previous != null)
        {
            selectidwaypoint.previous.next = selectidwaypoint.next;
            Selection.activeGameObject = selectidwaypoint.previous.gameObject;
        }
        Destroy(selectidwaypoint.gameObject);
    }
    void CreateBranch()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        waypoint Waypoint = waypointObject.GetComponent<waypoint>();

        waypoint branchFrom = Selection.activeGameObject.GetComponent<waypoint>();
        branchFrom.branches.Add(Waypoint);
        Waypoint.previous = branchFrom;
        Waypoint.transform.position = branchFrom.transform.position;
        Waypoint.transform.forward = branchFrom.transform.forward;
        Selection.activeGameObject = Waypoint.gameObject;
    }
}