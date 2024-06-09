using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerCollisions : MonoBehaviour
{
    public HealthManager healthManager;
    public LevelManager levelManager;
    public MenusManager MenusManager;
    private SoundsManager soundsManager;
    public EnergyManager energyManager;
    public CameraManager cameraManager;
    private PlayerMovement playerMovement;

    public ParticleSystem burningSmokeParticle;
    private ParticleSystem.EmissionModule burningSmokeParticleEmissionModule;
    private ParticleSystem.MainModule burningSmokeParticleMainModule;

    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI wrenchText;

    public static int lifeRemaining = 3;

    public GameObject playerDeathParticle;
    public GameObject speedPowerUpIcon;

    private CinemachineImpulseSource impulseSource;

    // Start is called before the first frame update
    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        soundsManager = FindObjectOfType<SoundsManager>();

        soundsManager.engineRunningAudioSource.Play();
        soundsManager.brakingAudioSource.Play();

        if (SceneManager.GetActiveScene().name != "Menu")
        {
            playerMovement = GetComponent<PlayerMovement>();
            burningSmokeParticleEmissionModule = burningSmokeParticle.emission;
            burningSmokeParticleMainModule = burningSmokeParticle.main;

            lifeText.text = "x0" + lifeRemaining;
            wrenchText.text = levelManager.wrenchCollected + "/" + levelManager.startRemainingWrenches;
        }
        else
        {
            lifeRemaining = 3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            if (!MenusManager.isGameWon)
            {
                healthManager.DamagePlayer(8f);
                burningSmokeParticleEmissionModule.rateOverTime = (int)((-healthManager.playerHealth + 1f) * 15f);

                if (healthManager.playerHealth < 0.3f)
                {
                    burningSmokeParticleMainModule.startColor = new ParticleSystem.MinMaxGradient(new Color(0.941f, 0.443f, 0), new Color(1, 0.804f, 0));
                }
                else if (healthManager.playerHealth > 0.3f)
                {
                    burningSmokeParticleMainModule.startColor = new ParticleSystem.MinMaxGradient(new Color(0.604f, 0.604f, 0.604f), new Color(0.264f, 0.264f, 0.264f));
                }
            }

            if (healthManager.playerHealth <= 0)
            {
                DestroyPlayer();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wrench"))
        {
            healthManager.HealPlayer(12f);
            levelManager.wrenchCollected++;
            wrenchText.text = levelManager.wrenchCollected + "/" + levelManager.startRemainingWrenches;

            Destroy(collision.gameObject);
            soundsManager.audioSource.PlayOneShot(soundsManager.wrenchCollectSound, 1f);
        }
        if (collision.gameObject.CompareTag("Energy PowerUp"))
        {
            StartCoroutine(EnergyPowerUp(collision));
            soundsManager.audioSource.PlayOneShot(soundsManager.wrenchCollectSound, 1f);
        }
        if (collision.gameObject.CompareTag("+1 Life"))
        {
            AddLife();
            soundsManager.audioSource.PlayOneShot(soundsManager.wrenchCollectSound, 1f);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Water") || collision.gameObject.CompareTag("Spike"))
        {
            healthManager.playerHealth = 0f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerMovement.forwardSpeed > 6f || playerMovement.forwardSpeed < -6f)
        {
            cameraManager.ShakeCamera(impulseSource, 0.1f);
        }
        soundsManager.audioSource.PlayOneShot(soundsManager.hitSound, playerMovement.forwardSpeed / 10f);
    }

    public void DestroyPlayer()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            if (!MenusManager.isGameWon)
            {
                RemoveLife();
            }
        }

        PlayerDestructionFX();

        soundsManager.engineRunningAudioSource.Stop();
        soundsManager.brakingAudioSource.Stop();

        Destroy(gameObject);
    }

    IEnumerator EnergyPowerUp(Collider2D energyPowerUp)
    {
        Destroy(energyPowerUp.gameObject);
        playerMovement.bonusSpeed = 6f;
        energyManager.energyAmount += 0.3f;
        speedPowerUpIcon.SetActive(true);
        yield return new WaitForSeconds(2f);
        playerMovement.bonusSpeed = 0f;
        speedPowerUpIcon.SetActive(false);
    }

    private void AddLife()
    {
        lifeRemaining++;
        lifeText.text = "x0" + lifeRemaining;
    }
    private void RemoveLife()
    {
        lifeRemaining--;
        lifeText.text = "x0" + lifeRemaining;
    }

    private void PlayerDestructionFX()
    {
        Instantiate(playerDeathParticle, transform.position, transform.rotation);
        cameraManager.ShakeCamera(impulseSource, 1f);

        soundsManager.audioSource.PlayOneShot(soundsManager.playerDeathSound, 1f);
    }
}
