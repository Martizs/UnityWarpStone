using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField]
    float shakeSpeed = 1f;

    [SerializeField]
    float shakeAmount = 1f;

    [SerializeField]
    float shakeRotateSpeed = 1f;

    [SerializeField]
    Vector3 shakeRotateVector;

    // Update is called once per frame
    void Update()
    {
        ShakeIt();
        RotationShake();
    }

    void ShakeIt()
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
