using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public Transform visualLeftWheel;
    public Transform visualRightWheel;
    public bool motor;
    public bool steering;
}

public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider, Transform visualWheel)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public virtual float vehicleMotor(float _maxMotorTorque)
    {
        return _maxMotorTorque;
    }

    public virtual float vehicleSteering(float _maxSteeringAngle)
    {
        return _maxSteeringAngle;
    }

    public virtual void AdditionalFeatures()
    {

    }

    public void FixedUpdate()
    {
        float motor = vehicleMotor(maxMotorTorque);
        float steering = vehicleSteering(maxSteeringAngle);

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel, axleInfo.visualLeftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel, axleInfo.visualRightWheel);
        }
    }

    private void Update()
    {
        AdditionalFeatures();
    }
}