using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CarController
{
    public float returnHeight;
    // lifeCost - value must be between 0 (0% life) and 1 (100% life)
    [Tooltip("How much life will be reduced. Goes from 0 to 100 (%)")]
    public float lifeCost;
    public HealthBarController healthBar;

    public override float vehicleMotor(float _maxMotorTorque)
    {
        float _motor = _maxMotorTorque * Input.GetAxis("Vertical");
        return _motor;
    }

    public override float vehicleSteering(float _maxSteeringAngle)
    {
        float _steering = _maxSteeringAngle * Input.GetAxis("Horizontal");
        return _steering;
    }

    public override void AdditionalFeatures()
    {
        if (Input.GetKeyDown("k"))
        {
            transform.rotation = Quaternion.identity;
            transform.position += new Vector3(0, returnHeight, 0);
            healthBar.DecreaseHealth(lifeCost / 100.0f);
        }
    }
}
