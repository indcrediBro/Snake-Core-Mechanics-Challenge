using System;
using UnityEngine;

public static class GameEvents
{
    public static Action OnFoodEaten;
    public static Action OnGameStart;
    public static Action OnGameOver;

    public static void FoodEaten() => OnFoodEaten?.Invoke();
    public static void GameStart() => OnGameStart?.Invoke();
    public static void GameOver() => OnGameOver?.Invoke();
}
