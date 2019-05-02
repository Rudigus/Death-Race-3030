﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputController : MonoBehaviour {

    [Header("Car Parameters")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float powerEngine;
    [SerializeField] private float brakePower;
    [SerializeField] private Transform COM;
    [SerializeField] private Wheels[] wheels;

	[Header("Sounds")]
	[SerializeField] private AudioClip StartEngineClip;
	[SerializeField] private AudioClip WorkingEngineClip;
	/*[HideInInspector]*/ public bool carInFocus;

	private Rigidbody rb;
    private AudioSource audioSource;
	private bool engineWorking;
	public bool ReadyMove { get {
			return engineWorking && carInFocus;
		}
	}

	[System.Serializable]
    public class Wheels{
        [Space(20), Header("Parametrs Wheel")]
        public WheelCollider wheelCollider;
		public GameObject wheelObject;
		public float angleTurningWheel;
        [Range(0, 100)]   public float percentMotorPower;
        [HideInInspector] public float wheelPower;

    }

    void Start () {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rb.centerOfMass = COM.localPosition;
        // Rudigus code!
        healthBar = GameObject.FindWithTag("HealthBar").GetComponent<HealthBarController>();
        speedBar = GameObject.FindWithTag("SpeedBar").GetComponent<SpeedBarController>();
    }

	public void StartEngine () {
		StartCoroutine("StartEngineCor");
	}

	public void StopEngine () {
		audioSource.Stop();
		engineWorking = false;
	}

	IEnumerator StartEngineCor () {
		if (StartEngineClip != null) {
			audioSource.clip = StartEngineClip;
			audioSource.Play();
			yield return new WaitForSeconds(StartEngineClip.length);
		}
		if (WorkingEngineClip != null) {
			audioSource.clip = WorkingEngineClip;
			audioSource.loop = true;
			audioSource.Play();
		}
		engineWorking = true;
	}
	void Update () {
		if (Input.GetKeyDown(KeyCode.E) && carInFocus) {
			if (engineWorking) {
				StopEngine();
			} else {
				StartEngine();
			}
		}
        //Rudigus function! Don't place it in FixedUpdate(), or else it will,
        // at least in my machine, run 2 times per key press.
        AdditionalFeatures();
	}
	void FixedUpdate() {
		Vector3 position;
		Quaternion rotation;
		currentSpeed = rb.velocity.magnitude;
		if (!carInFocus && currentSpeed < 0.01f) return;
		for (int i = 0; i < wheels.Length; i++) {
            if (wheels[i].wheelCollider == null) continue;
			if (currentSpeed > maxSpeed) {
				wheels[i].wheelPower = 0;
			} else {
				wheels[i].wheelPower = powerEngine * (wheels[i].percentMotorPower * 0.1f);
			}

			if (carInFocus) {
				if (engineWorking) {
					float inputVertical = Input.GetAxis("Vertical");
					if (wheels[i].wheelCollider.rpm < 0.01f && inputVertical < 0f || wheels[i].wheelCollider.rpm >= -0.01f && inputVertical >= 0f) {
						wheels[i].wheelCollider.brakeTorque = 0;
						wheels[i].wheelCollider.motorTorque = inputVertical * wheels[i].wheelPower;
					} else {
						wheels[i].wheelCollider.motorTorque = 0;
						WheelBrake(wheels[i].wheelCollider);
					}
				} else {
					wheels[i].wheelCollider.motorTorque = 0;
				}

				wheels[i].wheelCollider.steerAngle = Input.GetAxis("Horizontal") * wheels[i].angleTurningWheel;
				if (Input.GetAxis("Jump") != 0) {
					WheelBrake(wheels[i].wheelCollider);
				}
			} else {
				wheels[i].wheelCollider.motorTorque = 0;
			}

			wheels[i].wheelCollider.GetWorldPose(out position, out rotation);
			wheels[i].wheelObject.transform.position = position;
			wheels[i].wheelObject.transform.localPosition -= wheels[i].wheelCollider.center;
			wheels[i].wheelObject.transform.rotation = rotation;

			if (audioSource != null) {
                var speed = rb.velocity.magnitude;
                audioSource.pitch = 1 + (speed * 0.03f);
            }
        }
        SetSpeedBar();
    }

	void WheelBrake (WheelCollider wheelCollider) {
		wheelCollider.brakeTorque = brakePower;
	}

    //_From this part onwards, it's Rudigus territory!________________________________________

    public float returnHeight;
    [Tooltip("How much health will be reduced. Goes from 0 to 100 (%)")]
    public float healthCost;
    private HealthBarController healthBar;
    private SpeedBarController speedBar;
    private float damageTaken;
    float currentSpeed;
    private int pickUpCollected = 0;

    public void AdditionalFeatures()
    {
        if (Input.GetKeyDown("k"))
        {
            StopEngine();
            rb.velocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.position += new Vector3(0, returnHeight, 0);
            healthBar.DecreaseHealth(healthBar.HealthNormalize(healthCost));
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Vector3 collisionForce = col.impulse / Time.fixedDeltaTime;
        // And now you can use it for your calculations! - Thanks Serlite from StackOverflow!
        //print(collisionForce);
        damageTaken = (Mathf.Abs(collisionForce.x) + Mathf.Abs(collisionForce.y) + 
            Mathf.Abs(collisionForce.z))/ 1e7f * 100.0f / healthBar.maxHealth;
        //print(damageTaken);
        healthBar.DecreaseHealth(damageTaken);
    }

    private void SetSpeedBar()
    {
        speedBar.SetSpeed(speedBar.SpeedNormalize(currentSpeed));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            pickUpCollected++;
        }
    }

    public string GetPickUpCollected()
    {
        return pickUpCollected.ToString();
    }
}