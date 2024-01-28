using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    // So this is one complete cycle
    const float TAU = Mathf.PI * 2;

    Vector3 startingPosition;

    [SerializeField]
    Vector3 movementVector;

    [SerializeField]
    float period = 2f;

    [SerializeField]
    float shakeSpeed = 1f;

    [SerializeField]
    float shakeAmount = 1f;

    [SerializeField]
    float shakeRotateSpeed = 1f;

    [SerializeField]
    Vector3 shakeRotateVector;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ObstacleMovement();
        Shake();
        RotationShake();
    }

    void ObstacleMovement()
    {
        // continualy growing over time
        float cycles = Time.time / (period <= Mathf.Epsilon ? 1 : period);

        float rawSinWave = Mathf.Sin(cycles * TAU);

        // So that it would go from 0 to 1 instead of -1 to 1
        float movementFactor = (rawSinWave + 1f) / 2f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }

    void Shake()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount,
            transform.position.z
        );
    }

    void RotationShake()
    {
        transform.Rotate(shakeRotateVector * Mathf.Sin(Time.time * shakeRotateSpeed));
    }
}
