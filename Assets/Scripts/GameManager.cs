using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { MainMenu, Game, GameOver }
    private GameState currentState;

    private void OnEnable()
    {
        GameEvents.OnGameOver += GameOver;
    }
    private void OnDisable()
    {
        GameEvents.OnGameOver -= GameOver;
    }

    private void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.MainMenu:
                UIManager.Instance.ShowMainMenu();
                Time.timeScale = 0; // Pause the game
                break;
            case GameState.Game:
                UIManager.Instance.ShowGameUI();
                GameEvents.GameStart();
                Time.timeScale = 1; // Resume the game
                break;
            case GameState.GameOver:
                UIManager.Instance.ShowGameOver();
                Time.timeScale = 0;
                break;
        }
    }

    public void StartGame()
    {
        ChangeState(GameState.Game);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }
}
