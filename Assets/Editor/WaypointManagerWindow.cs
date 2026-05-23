using UnityEditor;
using UnityEngine;

public class WaypointManagerWindow : EditorWindow
{
    public Transform waypointOrigin;

    [MenuItem("Waypoints/Waypoints Editor Tools")]
    public static void ShowWindow()
    {
        WaypointManagerWindow window = GetWindow<WaypointManagerWindow>("Waypoints Editor Tools");
        window.Show();
    }

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("waypointOrigin"));

        if (waypointOrigin == null)
        {
            EditorGUILayout.HelpBox("Please assign a Waypoint Origin transform slot to initialize tools!", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            
            CreateButtons(); 
            
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    } // <-- This closes OnGUI completely!

    void CreateButtons()
    {
        if (GUILayout.Button("Create Waypoint", GUILayout.Height(40)))
        {
            CreateWaypoint(); 
        }
    }

    void CreateWaypoint()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointOrigin.childCount, typeof(Waypoint));

        waypointObject.transform.SetParent(waypointOrigin, false);
        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

        if (waypointOrigin.childCount > 1)
        {
            waypoint.previousWaypoint = waypointOrigin.GetChild(waypointOrigin.childCount - 2).GetComponent<Waypoint>();
            
            waypoint.previousWaypoint.nextWaypoint = waypoint;

            waypointObject.transform.position = waypoint.previousWaypoint.transform.position;
            waypointObject.transform.forward = waypoint.previousWaypoint.transform.forward;
        }

        Selection.activeGameObject = waypointObject;
    }
} // <-- This closes the entire Window Class perfectly!