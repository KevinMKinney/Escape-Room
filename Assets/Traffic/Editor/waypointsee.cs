using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]
public class waypointsee
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawGizmos(waypoint Waypoint, GizmoType gizmoType)
    {
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;
        } else
        {
            Gizmos.color = Color.blue * 0.5f;
        }
        Gizmos.DrawSphere(Waypoint.transform.position, 0.2f);
        Gizmos.DrawLine(Waypoint.transform.position + (Waypoint.transform.right * Waypoint.width / 2f), Waypoint.transform.position - (Waypoint.transform.right * Waypoint.width / 2f));
        if(Waypoint.previous != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = Waypoint.transform.right * Waypoint.width / 2f;
            Vector3 offsetTo = Waypoint.previous.transform.right * Waypoint.previous.width / 2f;

            Gizmos.DrawLine(Waypoint.transform.position + offset, Waypoint.previous.transform.position + offsetTo);
        }
        if (Waypoint.next != null)
        {
            Gizmos.color = Color.green;
            Vector3 offset = Waypoint.transform.right * -Waypoint.width / 2f;
            Vector3 offsetTo = Waypoint.next.transform.right * -Waypoint.next.width / 2f;

            Gizmos.DrawLine(Waypoint.transform.position + offset, Waypoint.next.transform.position + offsetTo);
        }
        if (Waypoint.branches != null)
        {
            foreach (waypoint branch in Waypoint.branches)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(Waypoint.transform.position, branch.transform.position);
            }
        }
    }
}
