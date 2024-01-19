using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    const string PRIORITY_LEFT = "left";
    const string PRIORITY_RIGHT = "right";

    Rigidbody warpStoneBody;
    AudioSource warpStoneAudio;

    [SerializeField]
    float speed = 1000f;

    [SerializeField]
    float rotationSpeed = 100f;

    [SerializeField]
    AudioClip warpSound;

    private bool thrusting = false;

    private bool rotatingLeft = false;
    private bool rotatingRight = false;

    private string priorityRotationKey = null;

    [SerializeField]
    ParticleSystem mainThrust;

    [SerializeField]
    ParticleSystem leftThrust;

    [SerializeField]
    ParticleSystem rightThrust;

    // Start is called before the first frame update
    void Start()
    {
        warpStoneAudio = GetComponent<AudioSource>();
        warpStoneBody = GetComponent<Rigidbody>();
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
        // but seems like using our own boolean value is a bit more concise
        if (!thrusting)
        {
            warpStoneAudio.PlayOneShot(warpSound);
            mainThrust.Play();
            thrusting = true;
        }
        warpStoneBody.AddRelativeForce(Vector3.up * Time.deltaTime * speed);
    }

    private void StopThrusting()
    {
        warpStoneAudio.Stop();
        mainThrust.Stop();
        thrusting = false;
    }

    void stopLeftRotaion()
    {
        rotatingLeft = false;
        rightThrust.Stop();
    }

    void stopRightRotation()
    {
        rotatingRight = false;
        leftThrust.Stop();
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
            rightThrust.Play();
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
            leftThrust.Play();
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
