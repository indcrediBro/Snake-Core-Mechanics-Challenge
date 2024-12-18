using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAutoHider : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    private void OnEnable()
    {
        GameEvents.OnGameStart += ShowSprite;
        GameEvents.OnGameOver += HideSprite;
    }
    private void OnDisable()
    {
        GameEvents.OnGameStart -= ShowSprite;
        GameEvents.OnGameOver -= HideSprite;
    }

    private void ShowSprite()
    {
        renderer.enabled = true;
    }

    private void HideSprite()
    {
        renderer.enabled = false;
    }
}
