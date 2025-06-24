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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        deathScreen.SetActive(false);
        gameActive = true;
        StartCoroutine(IncreaseScore());
    }
    public void EndRun()
    {
        deathScreen.SetActive(true);
        finalScoreText.text = "Final Score: " +  score.ToString();
    }
    IEnumerator IncreaseScore()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(0.05f);
            score += 3;
            scoreText.text = "Score: " + score.ToString();
        }
    }
    public void Restart(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}
