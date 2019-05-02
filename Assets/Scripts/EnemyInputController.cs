using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInputController : MonoBehaviour
{

    [Header("Car Parameters")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float powerEngine;
    [SerializeField] private float brakePower;
    [SerializeField] private Transform COM;
    [SerializeField] private Wheels[] wheels;

    [Header("Sounds")]
    [SerializeField] private AudioClip StartEngineClip;
    [SerializeField] private AudioClip WorkingEngineClip;

    private Rigidbody rb;
    private AudioSource audioSource;
    private bool engineWorking;

    [System.Serializable]
    public class Wheels
    {
        [Space(20), Header("Parametrs Wheel")]
        public WheelCollider wheelCollider;
        public GameObject wheelObject;
        public float angleTurningWheel;
        [Range(0, 100)] public float percentMotorPower;
        [HideInInspector] public float wheelPower;

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rb.centerOfMass = COM.localPosition;
        // Rudigus code!
        player = GameObject.FindWithTag("Player");
        enemySpawner = GameObject.FindWithTag("EnemySpawner");
        currentHealth = maxHealth;
    }

    public void StartEngine()
    {
        StartCoroutine("StartEngineCor");
    }

    public void StopEngine()
    {
        audioSource.Stop();
        engineWorking = false;
    }

    IEnumerator StartEngineCor()
    {
        if (StartEngineClip != null)
        {
            audioSource.clip = StartEngineClip;
            audioSource.Play();
            yield return new WaitForSeconds(StartEngineClip.length);
        }
        if (WorkingEngineClip != null)
        {
            audioSource.clip = WorkingEngineClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        engineWorking = true;
    }
    void Update()
    {
        if (!engineWorking)
        {
            StartEngine();
        }
        //Rudigus function! Don't place it in FixedUpdate(), or else it will,
        // at least in my machine, run 2 times per key press.
        //AdditionalFeatures();
        if (currentHealth <= 0.0f)
        {
            currentHealth = maxHealth;
            StopEngine();
            rb.velocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.position = GetSpawnPosition();
            StartEngine();
        }
    }
    void FixedUpdate()
    {
        Vector3 position;
        Quaternion rotation;
        currentSpeed = rb.velocity.magnitude;
        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i].wheelCollider == null) continue;
            if (currentSpeed > maxSpeed)
            {
                wheels[i].wheelPower = 0;
            }
            else
            {
                wheels[i].wheelPower = powerEngine * (wheels[i].percentMotorPower * 0.1f);
            }

            if (engineWorking)
            {
                float inputVertical = SetVerticalInput();
                if (wheels[i].wheelCollider.rpm < 0.01f && inputVertical < 0f || wheels[i].wheelCollider.rpm >= -0.01f && inputVertical >= 0f)
                {
                    wheels[i].wheelCollider.brakeTorque = 0;
                    wheels[i].wheelCollider.motorTorque = inputVertical * wheels[i].wheelPower;
                }
                else
                {
                    wheels[i].wheelCollider.motorTorque = 0;
                    WheelBrake(wheels[i].wheelCollider);
                }
            }
            else
            {
                wheels[i].wheelCollider.motorTorque = 0;
            }

            float inputHorizontal = SetHorizontalInput();

            wheels[i].wheelCollider.steerAngle = inputHorizontal * wheels[i].angleTurningWheel;
            if (Input.GetAxis("Jump") != 0)
            {
                WheelBrake(wheels[i].wheelCollider);
            }

            wheels[i].wheelCollider.GetWorldPose(out position, out rotation);
            wheels[i].wheelObject.transform.position = position;
            wheels[i].wheelObject.transform.localPosition -= wheels[i].wheelCollider.center;
            wheels[i].wheelObject.transform.rotation = rotation;

            if (audioSource != null)
            {
                var speed = rb.velocity.magnitude;
                audioSource.pitch = 1 + (speed * 0.03f);
            }
        }
    }

    void WheelBrake(WheelCollider wheelCollider)
    {
        wheelCollider.brakeTorque = brakePower;
    }

    //_From this part onwards, it's Rudigus territory!________________________________________

    private float damageTaken;
    private float currentSpeed;
    private GameObject player;
    private GameObject enemySpawner;
    private float distance;

    public Transform frontEdge;
    public Transform rearEdge;
    public Transform leftEdge;
    public Transform rightEdge;

    void OnCollisionEnter(Collision col)
    {
        Vector3 collisionForce = col.impulse / Time.fixedDeltaTime;
        // And now you can use it for your calculations! - Thanks Serlite from StackOverflow!
        //print(collisionForce);
        damageTaken = (Mathf.Abs(collisionForce.x) + Mathf.Abs(collisionForce.y) +
            Mathf.Abs(collisionForce.z)) / 1e7f;
        //print(damageTaken);
        currentHealth -= (damageTaken * 100f);
    }

    private float GetDistanceToPlayer()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        return distance;
    }

    // The maximum health in the game (%)
    public float maxHealth;
    private float currentHealth;
    public float aggressivityLevel;

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    private float SetVerticalInput()
    {
        float inputVertical;

        float frontDistance = Vector3.Distance(frontEdge.transform.position, 
            player.transform.position);
        float rearDistance = Vector3.Distance(rearEdge.transform.position,
            player.transform.position);
        float verticalDistance = Vector3.Distance(frontEdge.transform.position,
            rearEdge.transform.position);

        if (frontDistance > rearDistance)
        {
            inputVertical = Mathf.Lerp(aggressivityLevel * -1, -1.0f,
            (frontDistance - rearDistance) / verticalDistance);
        }
        else
        {
            inputVertical = Mathf.Lerp(aggressivityLevel, 1.0f,
            (rearDistance - frontDistance) / verticalDistance);
        }

        return inputVertical;
    }

    private float SetHorizontalInput()
    {
        float inputHorizontal;

        float leftDistance = Vector3.Distance(leftEdge.transform.position,
            player.transform.position);
        float rightDistance = Vector3.Distance(rightEdge.transform.position,
            player.transform.position);
        float horizontalDistance = Vector3.Distance(rightEdge.transform.position,
            leftEdge.transform.position);

        inputHorizontal = Mathf.Lerp(1.0f, -1.0f, 
            (rightDistance - leftDistance) / horizontalDistance);

        return inputHorizontal;
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 spawnPosition = new Vector3(0, 0, 0);
        for (int i = 0; i < enemySpawner.GetComponent<EnemySpawner>().enemies.Length; i++)
        {
            if (enemySpawner.GetComponent<EnemySpawner>().enemies[i].enemyCar == gameObject)
            {
                spawnPosition = 
                    enemySpawner.GetComponent<EnemySpawner>().enemies[i].spawnCoords;
                break;
            }
        }
        return spawnPosition;
    }
}
