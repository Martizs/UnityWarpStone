using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    const string PRIORITY_LEFT = "left";
    const string PRIORITY_RIGHT = "right";

    Rigidbody warpStoneBody;
    AudioSource warpStoneAudio;

    [SerializeField]
    float initialSpeed = 1000f;

    float speed;

    [SerializeField]
    float rotationSpeed = 100f;

    [SerializeField]
    AudioClip warpSound;

    private bool thrusting = false;

    private bool rotatingLeft = false;
    private bool rotatingRight = false;

    private string priorityRotationKey = null;

    [SerializeField]
    ParticleSystem[] mainThrusts;

    [SerializeField]
    float warpVFXSpeedIncrease = .5f;

    [SerializeField]
    float speedIncrease = .5f;

    float initialWarpSpeed = 1f;

    ParticleSystem.MainModule[] mainThrustModules;

    // Start is called before the first frame update
    void Start()
    {
        warpStoneAudio = GetComponent<AudioSource>();
        warpStoneBody = GetComponent<Rigidbody>();

        mainThrustModules = new ParticleSystem.MainModule[mainThrusts.Length];

        for (int i = 0; i < mainThrusts.Length; i++)
        {
            mainThrustModules[i] = mainThrusts[i].main;
        }

        // Doing this with the one since both should have the same speed
        initialWarpSpeed = mainThrustModules[0].simulationSpeed;

        speed = initialSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopThrusting();
        }
    }

    private void StartThrusting()
    {
        // Can use warpSoneAudio.isPLaying() && mainThrust.IsPlaying to reach a similar effect
        // but seems like using our own boolean value is a bit more conciseaa
        if (!thrusting)
        {
            warpStoneAudio.PlayOneShot(warpSound);
            StartPlayingThrusters();
            thrusting = true;
        }

        IncreaseThrusterSpeed();

        speed += speedIncrease * Time.deltaTime;

        warpStoneBody.AddRelativeForce(Vector3.up * Time.deltaTime * speed);
    }

    private void StartPlayingThrusters()
    {
        foreach (ParticleSystem mainThrust in mainThrusts)
        {
            mainThrust.Play();
        }
    }

    private void IncreaseThrusterSpeed()
    {
        for (int i = 0; i < mainThrustModules.Length; i++)
        {
            mainThrustModules[i].simulationSpeed += warpVFXSpeedIncrease * Time.deltaTime;
        }
    }

    private void StopPlayingThrusters()
    {
        for (int i = 0; i < mainThrustModules.Length; i++)
        {
            mainThrustModules[i].simulationSpeed = initialWarpSpeed;
            mainThrusts[i].Stop();
        }
    }

    public void StopThrusting()
    {
        StopPlayingThrusters();
        warpStoneAudio.Stop();
        speed = initialSpeed;
        thrusting = false;
    }

    void stopLeftRotaion()
    {
        rotatingLeft = false;
    }

    void stopRightRotation()
    {
        rotatingRight = false;
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A) && priorityRotationKey != PRIORITY_RIGHT)
        {
            RotateLeft();
        }

        if (Input.GetKey(KeyCode.D) && priorityRotationKey != PRIORITY_LEFT)
        {
            RotateRight();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            priorityRotationKey = null;
            stopLeftRotaion();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            priorityRotationKey = null;
            stopRightRotation();
        }
    }

    private void RotateLeft()
    {
        if (!rotatingLeft)
        {
            rotatingLeft = true;

            if (rotatingRight)
            {
                priorityRotationKey = PRIORITY_LEFT;
                stopRightRotation();
            }
        }
        DoRotation(true);
    }

    private void RotateRight()
    {
        if (!rotatingRight)
        {
            rotatingRight = true;

            if (rotatingLeft)
            {
                priorityRotationKey = PRIORITY_RIGHT;
                stopLeftRotaion();
            }
        }
        DoRotation(false);
    }

    private void DoRotation(bool forward)
    {
        // freeze rotation so we can manually rotate
        warpStoneBody.freezeRotation = true;
        transform.Rotate(
            forward
                ? Vector3.forward * Time.deltaTime * rotationSpeed
                : Vector3.back * Time.deltaTime * rotationSpeed
        );
        warpStoneBody.freezeRotation = false;
    }
}
