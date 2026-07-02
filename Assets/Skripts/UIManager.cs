using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Text Fields")]
    public TMP_Text scoreText;
    public TMP_Text ammoText;
    public TMP_Text bestScoreText;
    public TMP_Text waveText;

    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject mainMenuPanel;

    [Header("UI Audio")]
    public AudioSource uiAudioSource;
    public AudioClip buttonClickSound;

    private int currentScore = 0;
    private bool isPaused = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateScoreUI();
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        Time.timeScale = 0f;
        mainMenuPanel.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + currentScore;
    }

    public void UpdateAmmoUI(int current, int max)
    {
        ammoText.text = "" + current + "/" + max;
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UpdateWaveUI(int waveNumber)
    {
        waveText.text = "WAVE " + waveNumber;
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);

        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);

        if (currentScore > savedHighScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.Save();
            savedHighScore = currentScore;
        }

        bestScoreText.text = "Best Score: " + savedHighScore;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void PlayClickSound()
    {
        if (uiAudioSource != null && buttonClickSound != null)
        {
            uiAudioSource.PlayOneShot(buttonClickSound);
        }
    }
}
