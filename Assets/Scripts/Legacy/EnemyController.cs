using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : CarController
{
    public override float vehicleMotor(float _maxMotorTorque)
    {
        float _motor = _maxMotorTorque * Random.Range(0.0f, 1.0f);
        return _motor;
    }

    public override float vehicleSteering(float _maxSteeringAngle)
    {
        float _steering = _maxSteeringAngle * Random.Range(0.0f, 1.0f);
        return _steering;
    }
}