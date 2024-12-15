using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private TMP_Text[] scoresUI, bestScoresUI;

    private void OnEnable()
    {
        GameEvents.OnGameStart += UpdateScore;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= UpdateScore;
    }

    public void ShowMainMenu()
    {
        mainMenuUI.SetActive(true);
        gameUI.SetActive(false);
        gameOverUI.SetActive(false);
    }

    public void ShowGameUI()
    {
        mainMenuUI.SetActive(false);
        gameUI.SetActive(true);
        gameOverUI.SetActive(false);
    }

    public void ShowGameOver()
    {
        mainMenuUI.SetActive(false);
        gameUI.SetActive(false);
        gameOverUI.SetActive(true);
    }

    public void UpdateScore()
    {
        UpdateCurrentScoresUI();
        UpdateBestScoresUI();
    }

    private void UpdateCurrentScoresUI()
    {
        foreach (TMP_Text scoreText in scoresUI)
        {
            if (scoreText == null)
            {
                Debug.LogError("scoresUI contains a null reference!");
                continue;
            }

            scoreText.text = ScoreManager.Instance.Score.ToString();
        }
    }


    private void UpdateBestScoresUI()
    {
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        int currentScore = ScoreManager.Instance.Score;

        if (currentScore > highscore)
        {
            highscore = currentScore; // Update local highscore
            PlayerPrefs.SetInt("HighScore", highscore);
        }

        foreach (TMP_Text scoreText in bestScoresUI)
        {
            scoreText.text = highscore.ToString();
        }
    }

}
