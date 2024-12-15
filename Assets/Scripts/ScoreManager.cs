using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public int Score { get; private set; }

    private void OnEnable()
    {
        GameEvents.OnFoodEaten += IncreaseScore;
        GameEvents.OnGameStart += ResetScore;
    }

    private void OnDisable()
    {
        GameEvents.OnFoodEaten -= IncreaseScore;
        GameEvents.OnGameStart -= ResetScore;
    }

    private void IncreaseScore()
    {
        Score++;
        UIManager.Instance.UpdateScore();
    }

    private void ResetScore()
    {
        Score = 0;
    }
}
