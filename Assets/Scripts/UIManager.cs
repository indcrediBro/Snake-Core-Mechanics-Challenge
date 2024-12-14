using UnityEngine;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject gameOverUI;

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
}
