using UnityEngine;
using System.Collections;

public class DrivingSound : MonoBehaviour {

    private AudioSource engineSound;
    private const float LowPitch = 0.1f;
    private const float HighPitch = 2.0f;
    private const float SpeedToRevs = 0.02f;
    Rigidbody carRigidbody;
    public bool engineOn = false;

    void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
        //Audiosources Appear in the same Order as they do in the Inspector top to bottom
        engineSound = GetComponents<AudioSource>()[0];

    }

    private void FixedUpdate()
    {
        if (engineOn)
        {
            engineSound.loop = true;
            engineSound.Play();
            engineOn = false;
        }
        float forwardSpeed = transform.InverseTransformDirection(carRigidbody.velocity).z;
        float engineRevs = Mathf.Abs(forwardSpeed) * SpeedToRevs;
        engineSound.pitch = 1 + Mathf.Clamp(engineRevs, LowPitch, HighPitch);
    }
}
