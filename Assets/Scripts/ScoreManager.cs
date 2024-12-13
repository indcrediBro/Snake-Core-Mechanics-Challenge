using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public int Score { get; private set; }

    private void OnEnable()
    {
        GameEvents.OnFoodEaten += IncreaseScore;
    }

    private void OnDisable()
    {
        GameEvents.OnFoodEaten -= IncreaseScore;
    }

    private void IncreaseScore()
    {
        Score++;
        Debug.Log("Score: " + Score);
    }
}
