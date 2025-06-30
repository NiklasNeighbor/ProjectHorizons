using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    int score;
    public bool gameActive;
    [SerializeField]GameObject deathScreen;
    [SerializeField]TextMeshProUGUI scoreText;
    [SerializeField]TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    float highScore;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        deathScreen.SetActive(false);
        gameActive = true;
    }
    public void EndRun()
    {
        deathScreen.SetActive(true);

        highScore = PlayerPrefs.GetFloat("highScore", highScore);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat("highScore", highScore);
        }

        finalScoreText.text = "Final Score: " +  score.ToString();
        highScoreText.text = "High Score: " + PlayerPrefs.GetFloat("highScore", highScore);

    }
    public void IncreaseScore(int addedScore)
    {
        if(gameActive)
        {
            score += addedScore;
            scoreText.text = "Score: " + score.ToString();
        }
    }
    public void Restart(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}
