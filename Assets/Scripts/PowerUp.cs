﻿using UnityEngine;

public enum PowerUpType
{
    SpeedBoost,
    SlowMotion,
    ReverseControls,
    TailCutter
}

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;

    [SerializeField] private SpriteRenderer sRenderer;
    [SerializeField] private Sprite[] allSprites;
    [SerializeField] private float animationRate;

    private float timer;
    private int index = 0;

    private void Start()
    {
        type = (PowerUpType)Random.Range(0, 3);
    }

    private void LateUpdate()
    {
        AnimateSprite();
    }

    private void AnimateSprite()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            index++;
            if (index > allSprites.Length - 1)
            {
                index = 0;
            }

            sRenderer.sprite = allSprites[index];
            timer = animationRate;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Snake"))
        {
            GameEvents.FoodEaten();
            collision.GetComponent<SnakeController>().Grow();
            Destroy(gameObject);
        }
    }
}