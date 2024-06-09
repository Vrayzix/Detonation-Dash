using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float accelerationInput;
    public float accelerationFactor;
    public float bonusSpeed;
    public float maxSpeed;

    private float steeringInput;
    public float steeringFactor;
    private float minSpeedToSteer;
    private float rotationAngle;
    public float driftFactor = 0.9f;

    private Vector2 engineForceVector;
    public Vector2 forwardVelocity;
    public float forwardSpeed;
    public Vector2 rightVelocity;

    public MenusManager MenusManager;
    public EnergyManager energyManager;
    private SoundsManager soundsManager;
    public Joystick joystick;
    public GameObject joystickUI;

    public ParticleSystem pipeSmokeParticle;
    public ParticleSystem pipeBoostedSmokeParticle;

    public float minEnginePitch;
    public float maxEnginePitch;

    private float consumeFactor = 2.5f;


    void Start()
    {
        soundsManager = FindObjectOfType<SoundsManager>();
        rb = GetComponent<Rigidbody2D>();

        rotationAngle = transform.eulerAngles.z;

#if UNITY_ANDROID || UNITY_IOS
        steeringFactor = 3;
        joystickUI.SetActive(true);
#else
        joystickUI.SetActive(false);
#endif
    }


    void Update()
    {
        accelerationInput = Input.GetAxisRaw("Vertical");
        steeringInput = Input.GetAxisRaw("Horizontal");
#if UNITY_ANDROID || UNITY_IOS
        accelerationInput = joystick.Vertical;
        steeringInput = joystick.Horizontal;
#endif

        EngineSound();
        BrakeSound();


        // Debug Visualization
        Debug.DrawRay(transform.position, rb.velocity, Color.black);
        Debug.DrawRay(transform.position, forwardVelocity, Color.green);
        Debug.DrawRay(transform.position, rightVelocity, Color.red);

    }
    private void FixedUpdate()
    {
        ApplyEngineForce();
        ApplySteering();
        KillOrthogonalVelovity();
        //CheckBoundaries();
    }
    private void ApplyEngineForce()
    {
        forwardSpeed = Vector2.Dot(transform.up, rb.velocity);

        if (accelerationInput > 0)
        {
            energyManager.ConsumeEnergy(accelerationInput + (forwardSpeed - bonusSpeed) * consumeFactor);
        }
        else if (accelerationInput < 0)
        {
            energyManager.ConsumeEnergy(accelerationInput + (-forwardSpeed - bonusSpeed) * consumeFactor);
        }
        else
        {
            energyManager.RechargeEnergy(10f);
        }

        if (energyManager.energyAmount <= 0)
        {
            rb.drag = Mathf.Lerp(rb.drag, 3.0f, 3 * Time.fixedDeltaTime);
        }

        if (forwardSpeed > maxSpeed && accelerationInput > 0)
        {
            return;
        }
        if (forwardSpeed < -maxSpeed * 0.5f && accelerationInput < 0)
        {
            return;
        }
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
        {
            return;
        }

        engineForceVector = transform.up * accelerationInput * (accelerationFactor + bonusSpeed);
        if (!MenusManager.isGameWon && energyManager.energyAmount > 0.05 && accelerationInput != 0)
        {
            rb.AddForce(engineForceVector, ForceMode2D.Force);
            rb.drag = 0;
        }
        else if (accelerationInput == 0)
        {
            rb.drag = Mathf.Lerp(rb.drag, 3.0f, 3 * Time.fixedDeltaTime);
        }

        SmokeParticle();
    }
    private void ApplySteering()
    {
        minSpeedToSteer = Mathf.Clamp01(rb.velocity.magnitude / 8);
        rotationAngle -= steeringInput * steeringFactor * minSpeedToSteer;

        rb.MoveRotation(rotationAngle);
    }
    private void KillOrthogonalVelovity()
    {
        forwardVelocity = transform.up * Vector2.Dot(transform.up, rb.velocity);
        rightVelocity = transform.right * Vector2.Dot(transform.right, rb.velocity);
        //Debug.Log("forwardDot:" + Vector2.Dot(transform.up, rb.velocity));
        //Debug.Log("rightDot:" + Vector2.Dot(transform.right, rb.velocity));
        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    private void SmokeParticle()
    {
        if (accelerationInput > 0 && energyManager.energyAmount > 0)
        {
            if (forwardSpeed < 8 && !pipeSmokeParticle.isPlaying)
            {
                pipeSmokeParticle.Play();
                if (pipeBoostedSmokeParticle.isPlaying)
                {
                    pipeBoostedSmokeParticle.Stop();
                }
            }
            else if (forwardSpeed > 8 && !pipeBoostedSmokeParticle.isPlaying)
            {
                pipeBoostedSmokeParticle.Play();
                if (pipeSmokeParticle.isPlaying)
                {
                    pipeSmokeParticle.Stop();
                }
            }
        }
        else if (accelerationInput == 0 || accelerationInput > 0 && energyManager.energyAmount <= 0)
        {
            if (pipeSmokeParticle.isPlaying)
            {
                pipeSmokeParticle.Stop();
            }
            else if (pipeBoostedSmokeParticle.isPlaying)
            {
                pipeBoostedSmokeParticle.Stop();
            }
        }
    }

    private void EngineSound()
    {
        soundsManager.engineRunningAudioSource.pitch = Mathf.Lerp(minEnginePitch, maxEnginePitch, rb.velocity.magnitude / 20f);

        if (MenusManager.isGamePaused)
        {
            soundsManager.engineRunningAudioSource.Pause();
        }
        else
        {
            soundsManager.engineRunningAudioSource.UnPause();
        }
    }

    private void BrakeSound()
    {
        if (forwardSpeed > 5 && accelerationInput < 0)
        {
            soundsManager.brakingAudioSource.volume = Mathf.Lerp(soundsManager.brakingAudioSource.volume, 1f, Time.fixedDeltaTime * 10);
        }
        else
        {
            soundsManager.brakingAudioSource.volume = Mathf.Lerp(soundsManager.brakingAudioSource.volume, 0f, Time.fixedDeltaTime * 10);
        }
    }
}
