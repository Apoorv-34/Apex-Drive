using System.Numerics;
using UnityEngine;

public enum CarType { FrontWheelDrive, RearWheelDrive, FourWheelDrive }
public enum ControlMode { Keyboard, Button }

public class CarController : MonoBehaviour
{
    public ControlMode control;
    public CarType carType = CarType.FourWheelDrive;

    [Header("Wheel Meshes")]
    public GameObject frontWheelLeft;
    public GameObject frontWheelRight;
    public GameObject backWheelLeft;
    public GameObject backWheelRight;

    [Header("Wheel Colliders")]
    public WheelCollider frontWheelLeftCollider;
    public WheelCollider frontWheelRightCollider;
    public WheelCollider backWheelLeftCollider;
    public WheelCollider backWheelRightCollider;

    [Header("Settings")]
    public float maximumMotorTorque = 900f;
    public float maximumSteeringAngle = 20f;
    public float maximumSpeed = 140f;
    public float brakePower = 5000f;
    public Transform centerOfMass;

    private Rigidbody carRigidbody;
    private float motorTorque;
    private float vertical;
    private float horizontal;
    private float carSpeed;
    private float carSpeedConverted;
    private bool handbrake = false;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        if (carRigidbody != null && centerOfMass != null)
        {
            carRigidbody.centerOfMass = centerOfMass.localPosition;
        }
    }

    void Update()
    {
        GetInputs();
        CalculateCarMovement();
        CalculateSteering();
        ApllyTransformToWheels();
    }

    void GetInputs()
    {
        if (control == ControlMode.Keyboard)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
    }

    void CalculateCarMovement()
    {
        carSpeed = carRigidbody.linearVelocity.magnitude;
        carSpeedConverted = Mathf.Round(carSpeed * 3.6f); // Convert m/s to km/h

        if (Input.GetKey(KeyCode.Space))
            handbrake = true;
        else
            handbrake = false;

        if (handbrake)
        {
            motorTorque = 0;
            ApplyBrake();
        }
        else
        {
            ReleaseBrake();
            if (carSpeedConverted < maximumSpeed)
            {
                motorTorque = maximumMotorTorque * vertical;
            }
            else
            {
                motorTorque = 0;
            }
        }
        ApplyMotorTorque();
    }

    void CalculateSteering()
    {
        float tireAngle = maximumSteeringAngle * horizontal;
        frontWheelLeftCollider.steerAngle = tireAngle;
        frontWheelRightCollider.steerAngle = tireAngle;
    }

    void ApplyMotorTorque()
    {
        if (carType == CarType.FrontWheelDrive)
        {
            frontWheelLeftCollider.motorTorque = motorTorque;
            frontWheelRightCollider.motorTorque = motorTorque;
        }
        else if (carType == CarType.RearWheelDrive)
        {
            backWheelLeftCollider.motorTorque = motorTorque;
            backWheelRightCollider.motorTorque = motorTorque;
        }
        else if (carType == CarType.FourWheelDrive)
        {
            frontWheelLeftCollider.motorTorque = motorTorque;
            frontWheelRightCollider.motorTorque = motorTorque;
            backWheelLeftCollider.motorTorque = motorTorque;
            backWheelRightCollider.motorTorque = motorTorque;
        }
    }

    void ApplyBrake()
    {
        frontWheelLeftCollider.brakeTorque = brakePower;
        frontWheelRightCollider.brakeTorque = brakePower;
        backWheelLeftCollider.brakeTorque = brakePower;
        backWheelRightCollider.brakeTorque = brakePower;
    }

    void ReleaseBrake()
    {
        frontWheelLeftCollider.brakeTorque = 0;
        frontWheelRightCollider.brakeTorque = 0;
        backWheelLeftCollider.brakeTorque = 0;
        backWheelRightCollider.brakeTorque = 0;
    }
    public void ApllyTransformToWheels()
    {
        UnityEngine.Vector3 position;
        UnityEngine.Quaternion rotation;
        frontWheelLeftCollider.GetWorldPose(out position, out rotation);
        frontWheelLeft.transform.position = position;
        frontWheelLeft.transform.rotation = rotation;
        frontWheelRightCollider.GetWorldPose(out position, out rotation);
        frontWheelRight.transform.position = position;
        frontWheelRight.transform.rotation = rotation;
        backWheelLeftCollider.GetWorldPose(out position, out rotation);
        backWheelLeft.transform.position = position; 
        backWheelLeft.transform.rotation = rotation;
        backWheelRightCollider.GetWorldPose(out position, out rotation);
        backWheelRight.transform.position = position;
        backWheelRight.transform.rotation = rotation;
    }
}