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
    [SerializeField] private Vector2 min, max;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveInterval = 2f;
    private Vector3 targetPosition;
    private bool moving;

    [SerializeField] private float detachForce = 5f; // Force applied to detached parts

    private void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        SetInitialPosition(); // Initialize the position correctly
        InvokeRepeating(nameof(MoveToRandomPosition), moveInterval, moveInterval);
    }

    private void SetInitialPosition()
    {
        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);
        transform.position = new Vector3(x, y, transform.position.z);
    }

    private void MoveToRandomPosition()
    {
        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);
        targetPosition = new Vector3(x, y, transform.position.z);
        moving = true;
    }

    private void LateUpdate()
    {
        if (moving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                moving = false;
            }
        }
        AnimateSprite();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle")) // Ignore collisions with obstacles
        {
            MoveToRandomPosition();
        }
        else if (collision.collider.CompareTag("Snake"))
        {
            GameEvents.FoodEaten();
            collision.collider.GetComponent<SnakeController>().Grow(); // Only grow once per collision
            DetachBodyParts();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Snake"))
        {
            GameEvents.FoodEaten();
            other.GetComponent<SnakeController>().Grow(); // Only grow once per collision
            DetachBodyParts();
        }
        else
        {
            MoveToRandomPosition();
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
        Destroy(gameObject);
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
    }
}
