using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Waypoint Info")]
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;

    [Range(0f, 5f)]
    public float waypointWidth = 5f;

    /// <summary>
    /// Computes a random world position distributed evenly across the width of the track.
    /// This prevents AI cars from driving in a rigid single file line.
    /// </summary>
    public Vector3 GetPosition()
    {
        // 1. Calculate the left-hand boundary node of the track lane
        Vector3 minBound = transform.position + (transform.right * (waypointWidth / 2f));

        // 2. Calculate the right-hand boundary node of the track lane
        Vector3 maxBound = transform.position - (transform.right * (waypointWidth / 2f));

        // 3. Linearly interpolate a random coordinate point directly between the two boundaries
        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}