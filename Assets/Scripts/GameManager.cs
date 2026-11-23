using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public int lives = 3;
    public bool isGameOver = false;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public GameObject endPanel;
    public TextMeshProUGUI endText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }
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

        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
    }
}
