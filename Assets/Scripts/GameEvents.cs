using System;
using UnityEngine;

public static class GameEvents
{
    public static Action OnFoodEaten;
    public static Action OnGameOver;

    public static void FoodEaten() => OnFoodEaten?.Invoke();
    public static void GameOver()
    {
        OnGameOver?.Invoke();
        Debug.Log("Game Over triggered");
    }
}
