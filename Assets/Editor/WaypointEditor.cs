using UnityEditor;
using UnityEngine;

[InitializeOnLoad] // Forces the script to run seamlessly in the Editor background without hitting Play
public class WaypointEditor 
{
    // Tells Unity to call this method to draw gizmos for both selected and unselected waypoints in the Scene
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmos(Waypoint waypoint, GizmoType gizmoType)
    {
        // === 1. DRAW 3D SPHERES FOR EACH CHECKPOINT ===
        // Flashes bright solid blue if you click on it, semi-transparent blue if unselected
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.blue * 0.5f; // Semi-transparent blue
        }

        // Spawns a clean anchor node sphere inside your 3D Scene View grid
        Gizmos.DrawSphere(waypoint.transform.position, 0.5f);


        // === 2. DRAW WHITE TRACK WIDTH LANE BARRIERS ===
        Gizmos.color = Color.white;
        
        // Dynamically draws horizontal guidelines extending across your track road mesh
        Vector3 widthLineStart = waypoint.transform.position + (waypoint.transform.right * (waypoint.waypointWidth / 2f));
        Vector3 widthLineEnd = waypoint.transform.position - (waypoint.transform.right * (waypoint.waypointWidth / 2f));
        Gizmos.DrawLine(widthLineStart, widthLineEnd);


        // === 3. DRAW PATHWAY LINK CONNECTIONS BETWEEN NODES ===
        // Draw a RED track boundary line tracing backward along your path link map
        if (waypoint.previousWaypoint != null)
        {
            Gizmos.color = Color.red;
            
            Vector3 offset = waypoint.transform.right * (waypoint.waypointWidth / 2f);
            Vector3 offsetPrevious = waypoint.previousWaypoint.transform.right * (waypoint.previousWaypoint.waypointWidth / 2f);
            
            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.previousWaypoint.transform.position + offsetPrevious);
        }

        // Draw a GREEN track boundary line tracing forward along your path link map
        if (waypoint.nextWaypoint != null)
        {
            Gizmos.color = Color.green;
            
            Vector3 offset = waypoint.transform.right * (waypoint.waypointWidth / 2f);
            Vector3 offsetNext = waypoint.nextWaypoint.transform.right * (waypoint.nextWaypoint.waypointWidth / 2f);
            
            Gizmos.DrawLine(waypoint.transform.position - offset, waypoint.nextWaypoint.transform.position - offsetNext);
        }
    }
}