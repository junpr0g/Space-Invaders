using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public int lives = 3;
    public bool isGameStarted = false;
    public bool isWaveRespawning = false;
    public bool isGameOver = false;
    public bool blockShootThisFrame = false;

    public TextMeshProUGUI scoreText;
    public GameObject[] lifeIcons;
    public GameObject startPanel;
    public GameObject endPanel;
    public TextMeshProUGUI endText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();

        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }

        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!isGameStarted && !isGameOver)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                isGameStarted = true;
                blockShootThisFrame = true;

                if (startPanel != null)
                {
                    startPanel.SetActive(false);
                }
            }
        }
    }

    void LateUpdate()
    {
        blockShootThisFrame = false;
    }

    public bool CanPlay()
    {
        return isGameStarted && !isWaveRespawning && !isGameOver;
    }

    public void AddScore(int points)
    {
        if (isGameOver)
        {
            return;
        }

        score += points;
        UpdateUI();
    }

    public void LoseLife()
    {
        if (isGameOver)
        {
            return;
        }

        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            GameOver(false);
        }
    }

    public void GameOver(bool win)
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;

        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }

        if (endText != null)
        {
            if (win)
            {
                endText.text = "YOU WIN!";
            }
            else
            {
                endText.text = "GAME OVER";
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        if (lifeIcons != null)
        {
            for (int i = 0; i < lifeIcons.Length; i++)
            {
                if (lifeIcons[i] != null)
                {
                    lifeIcons[i].SetActive(i < lives);
                }
            }
        }
    }
}
