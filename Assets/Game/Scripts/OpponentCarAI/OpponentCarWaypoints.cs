using UnityEngine;

public class OpponentCarWaypoints : MonoBehaviour
{
    [Header("Opponent AI Settings")]
    public OpponentCar opponentCar;
    public Waypoint currentWaypoint;

    void Start()
    {
        // Fire up initial trajectory vectors targeted at node index 0
        opponentCar.LocateDestination(currentWaypoint.GetPosition());
    }

    void Update()
    {
        // If the motor script hits its destination flag, feed it the next path node
        if (opponentCar.destinationReached)
        {
            // Update node index tracking step
            currentWaypoint = currentWaypoint.nextWaypoint;

            // Push fresh destination vector arrays forward down the road path loop
            opponentCar.LocateDestination(currentWaypoint.GetPosition());
        }
    }
}