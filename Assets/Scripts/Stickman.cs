using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickman : MonoBehaviour
{
    [SerializeField] private Sprite[] allSprites;
    [SerializeField] private float animationRate;

    [SerializeField] private SpriteRenderer sRenderer;
    private float timer;
    private int index = 0;

    [SerializeField] private float detachForce = 5f; // Force applied to detached parts

    private void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        AnimateSprite();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Snake"))
        {
            GameEvents.FoodEaten();
            DetachBodyParts();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Snake"))
        {
            GameEvents.FoodEaten();
            DetachBodyParts();
        }
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

    private void DetachBodyParts()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.SetParent(null);
            child.gameObject.SetActive(true);

            // Check if the child has a Rigidbody2D component
            Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Apply a random force in 2D space
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                rb.AddForce(randomDirection * detachForce, ForceMode2D.Impulse);
            }
        }
        Destroy(gameObject);
    }
}
