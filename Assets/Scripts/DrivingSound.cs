using UnityEngine;
using System.Collections;

public class DrivingSound : MonoBehaviour {

    public AudioSource engineSound;
    private const float LowPitch = 0.1f;
    private const float HighPitch = 2.0f;
    private const float SpeedToRevs = 0.01f;
    Rigidbody carRigidbody;

    void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float forwardSpeed = transform.InverseTransformDirection(carRigidbody.velocity).z;
        float engineRevs = Mathf.Abs(forwardSpeed) * SpeedToRevs;
        engineSound.pitch = 1 + Mathf.Clamp(engineRevs, LowPitch, HighPitch);
    }
}
