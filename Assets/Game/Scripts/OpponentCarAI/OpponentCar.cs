using UnityEngine;

public class OpponentCar : MonoBehaviour
{
    [Header("Car Engine")]
    public float maxSpeed = 35f;
    public float acceleration = 3.5f;
    public float turningSpeed = 17f;
    public float breakSpeed = 12f;
    public float currentSpeed;

    [Header("Destination Variables")]
    public Vector3 destination;
    public bool destinationReached;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    void Update()
    {
        Drive();
    }

    void Drive()
    {
        if (destinationReached) return;

        // 1. Calculate direction vector mapping heading targets
        Vector3 destinationDirection = destination - transform.position;
        destinationDirection.y = 0; // Flat alignment mapping tracking coordinates
        
        float destinationDistance = destinationDirection.magnitude;

        // 2. Drive mode check: If far enough, keep moving forward
        if (destinationDistance >= breakSpeed)
        {
            // Smoothly rotate tracking angles towards destination heading
            Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);

            // Linearly escalate car internal velocity parameters over time slices
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
            rb.linearVelocity = transform.forward * currentSpeed;
        }
        else
        {
            // 3. Arrival check: Arrived at current point node boundary limits
            destinationReached = true;
            rb.linearVelocity = Vector3.zero;
        }
    }

    // Public receiver block used by the navigator script to push fresh targets
    public void LocateDestination(Vector3 targetPos)
    {
        destination = targetPos;
        destinationReached = false;
    }
}