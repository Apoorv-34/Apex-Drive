using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Smoothness Settings")]
    public float moveSmoothness = 25f;
    public float rotationSmoothness = 25f;

    [Header("Offsets")]
    public Vector3 moveOffset;
    public Vector3 rotationOffset;

    [Header("Target Link")]
    public Transform carTarget;

    // LateUpdate runs after all regular Update calls, ensuring smooth tracking
    void LateUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        // 1. Calculate where the target position should be relative to the car
        Vector3 targetPosition = carTarget.TransformPoint(moveOffset);

        // 2. Smoothly slide the camera toward that position over time
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSmoothness * Time.deltaTime);
    }

    void HandleRotation()
    {
        // 1. Determine the look direction from the camera to the car
        Vector3 direction = carTarget.position - transform.position;

        // 2. Formulate a look rotation incorporating our offset tilt and the upward direction
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // 3. Smoothly spin the camera toward that looking angle
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
    }
}