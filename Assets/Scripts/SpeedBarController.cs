using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBarController : MonoBehaviour
{
    public Transform bar;
    private GameObject player;
    public float maxSpeed;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        SetSpeed(0.0f);
    }

    // Remember, the speed will never be negative, because Rigidbody.velocity.magnitude
    // involves squaring in its equation, so the minimum speed here will be 0.
    public void SetSpeed(float speedNormalized)
    {
        if (speedNormalized < 1.0f)
        {
            bar.localScale = new Vector2(1.0f, speedNormalized);
        }
        else
        {
            bar.localScale = Vector2.one;
            Debug.Log("The speed is greater than the speed meter, and that should not " +
                "happen, please look into it!");
        }
    }

    public float SpeedNormalize(float speedUnnormalized)
    {
        float speedNormalized = 
            speedUnnormalized / maxSpeed;
        return speedNormalized;
    }
}
