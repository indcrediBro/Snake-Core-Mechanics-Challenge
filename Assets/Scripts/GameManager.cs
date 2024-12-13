using UnityEngine;
using System;

public enum GameState
{
    MainMenu,
    InGame,
    GameOver,
    Paused,
    Victory
}

public class GameManager : Singleton<GameManager>
{
    // Delegate to trigger events on game state change
    public static event Action<GameState> OnGameStateChanged;

    // Current game state
    private GameState currentState;
    private bool isPaused;

    private void Start()
    {
        // Set initial state (Main Menu by default)
        SetGameState(GameState.MainMenu);
    }

    // Public method to change the game state
    public void SetGameState(GameState newState)
    {
        // Exit if the new state is the same as the current one
        if (newState == currentState) return;

        currentState = newState;

        // Invoke the event to notify other systems of the state change
        OnGameStateChanged?.Invoke(currentState);

        // Handle game state transitions
        HandleGameStateChange(currentState);
    }

    // Private method that handles the game state logic
    private void HandleGameStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                LoadMainMenu();
                break;

            case GameState.InGame:
                StartGame();
                break;

            case GameState.GameOver:
                HandleGameOver();
                break;

            case GameState.Paused:
                PauseGame();
                break;

            case GameState.Victory:
                HandleVictory();
                break;

            default:
                Debug.LogWarning("Unknown game state: " + newState);
                break;
        }
    }

    // Method to load the Main Menu scene
    private void LoadMainMenu()
    {
        SceneLoader.Instance.LoadScene("MainMenu");
        Time.timeScale = 1f; // Ensure the game is not paused
    }

    // Method to start or resume the game
    private void StartGame()
    {
        SceneLoader.Instance.LoadScene("Game");
        Time.timeScale = 1f; // Ensure normal game speed
    }

    // Method to handle the Game Over state
    private void HandleGameOver()
    {
        UIManager.Instance.OpenPanel("GameOver");
        Time.timeScale = 1f; // Ensure the game is not paused
    }

    // Method to handle the Paused state
    private void PauseGame()
    {
        if (!isPaused)
        {
            UIManager.Instance.OpenPanel("PauseScreen");
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            UnpauseGame();
        }
    }

    // Method to handle the UnPaused state
    private void UnpauseGame()
    {
        UIManager.Instance.CloseActivePanel();
        Time.timeScale = 1f; // Pause the game
    }

    // Public method to restart the current scene (for Retry or Replay functionality)
    public void RestartGame()
    {
        SceneLoader.Instance.ReloadCurrentScene();
        Time.timeScale = 1f; // Reset time scale in case the game was paused
    }

    private void HandleVictory()
    {
        UIManager.Instance.OpenPanel("VictoryScreen");
        Time.timeScale = 1f; // Ensure the game is not paused
    }

    // Public method to quit the game (this works for builds, not the editor)
    public void QuitGame()
    {
        Application.Quit();
    }
}
