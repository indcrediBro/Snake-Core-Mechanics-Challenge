using System;

public static class GameEvents
{
    public static Action OnFoodEaten;
    public static Action OnSnakeHitWall;

    public static void FoodEaten() => OnFoodEaten?.Invoke();
    public static void SnakeHitWall() => OnSnakeHitWall?.Invoke();
}
