using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenusManager : MonoBehaviour
{
    public HealthManager healthManager;
    public LevelManager levelManager;
    public SoundsManager soundsManager;
    private PlayerCollisions playerCollisions;
    public GameObject pauseScreen;
    public GameObject gameOverScreen;
    public GameObject gameWonScreen;
    public GameObject menuScreen;
    public Button playButton;
    public Button restartButton;
    public Button nextButton;
    public Button menuButton;
    public Button resumeButton;
    public Toggle SFXToggle;
    public Toggle BGMToggle;
    public Image SFXSelected;
    public Image BGMSelected;
    public bool isGamePaused = false;
    public bool isGameWon = false;

    private void Start()
    {
        soundsManager = FindObjectOfType<SoundsManager>();

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            playerCollisions = FindObjectOfType<PlayerCollisions>();
            soundsManager.BGMAudioSource.clip = soundsManager.BGMMusicv1;
            soundsManager.BGMAudioSource.Play();
            EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        }
        else if (SceneManager.GetActiveScene().name == "Demo End")
        {
            soundsManager.engineRunningAudioSource.Stop();
            soundsManager.BGMAudioSource.clip = soundsManager.BGMMusicv2;
            soundsManager.BGMAudioSource.Play();
            EventSystem.current.SetSelectedGameObject(menuButton.gameObject);
        }
        isGameWon = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && SceneManager.GetActiveScene().name != "Menu" && SceneManager.GetActiveScene().name != "Demo End")
        {
            if (!isGamePaused)
            {
                PauseGame();
            }
            else if (isGamePaused)
            {
                ResumeGame();
            }
        }

        if (SceneManager.GetActiveScene().name != "Menu" && SceneManager.GetActiveScene().name != "Demo End")
        {
            if (healthManager.healthBar.fillAmount <= 0 && isGameWon == false)
            {
                GameOver();
            }

            if (levelManager.Wrenches.Length <= 0)
            {
                isGameWon = true;
                GameWon();
            }
        }
        if (SceneManager.GetActiveScene().name != "Demo End")
        {
            if (PlayerPrefs.GetString("SFXState") == "Off")
            {
                SFXToggle.isOn = true;
            }
            else if (PlayerPrefs.GetString("SFXState") == "On")
            {
                SFXToggle.isOn = false;
            }
            if (PlayerPrefs.GetString("BGMState") == "Off")
            {
                BGMToggle.isOn = true;
            }
            else if (PlayerPrefs.GetString("BGMState") == "On")
            {
                BGMToggle.isOn = false;
            }
            if (EventSystem.current.currentSelectedGameObject == SFXToggle.gameObject)
            {
                SFXSelected.gameObject.SetActive(true);
            }
            else if (EventSystem.current.currentSelectedGameObject != SFXToggle.gameObject)
            {
                SFXSelected.gameObject.SetActive(false);
            }
            if (EventSystem.current.currentSelectedGameObject == BGMToggle.gameObject)
            {
                BGMSelected.gameObject.SetActive(true);
            }
            else if (EventSystem.current.currentSelectedGameObject != BGMToggle.gameObject)
            {
                BGMSelected.gameObject.SetActive(false);
            }
        }
    }

    public void StartGame()
    {
        if (GameObject.Find("Player"))
        {
            playerCollisions.DestroyPlayer();
            StartCoroutine(StartDelay());
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if (!pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
            isGamePaused = true;
            Time.timeScale = 0f;
        }    }
    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        isGamePaused = false;
        Time.timeScale = 1f;
    }

    private void GameWon()
    {
        if (!gameWonScreen.activeSelf)
        {
            gameWonScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(nextButton.gameObject);
        }
    }
    private void GameOver()
    {
        if (!gameOverScreen.activeSelf)
        {
            gameOverScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(restartButton.gameObject);
        }
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (PlayerCollisions.lifeRemaining <= 0)
        {
            PlayerCollisions.lifeRemaining = 3;
            SceneManager.LoadScene("Level 1");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Level 1");
    }

    public void SFXToggleHandler(bool mute)
    {
        if (mute)
        {
            soundsManager.audioSource.mute = true;
            soundsManager.engineRunningAudioSource.mute = true;
            soundsManager.brakingAudioSource.mute = true;
            PlayerPrefs.SetString("SFXState", "Off");
        }
        else if (!mute)
        {
            soundsManager.audioSource.mute = false;
            soundsManager.engineRunningAudioSource.mute = false;
            soundsManager.brakingAudioSource.mute = false;
            PlayerPrefs.SetString("SFXState", "On");
        }
    }
    public void BGMToggleHandler(bool mute)
    {
        if (mute)
        {
            soundsManager.BGMAudioSource.mute = true;
            PlayerPrefs.SetString("BGMState", "Off");
        }
        else if (!mute)
        {
            soundsManager.BGMAudioSource.mute = false;
            PlayerPrefs.SetString("BGMState", "On");
        }
    }
}
