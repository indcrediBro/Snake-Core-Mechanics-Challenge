using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private float moveInterval = 0.2f; // Time between each movement step
    [SerializeField] private float gridSize = 1f; // Size of each movement step (grid size)
    [SerializeField] private GameObject snakeBodyPrefab;

    private Vector2 currentDirection = Vector2.right; // Initial movement direction
    private Vector2 nextDirection = Vector2.right;
    private List<Transform> snakeSegments = new List<Transform>();
    private List<Vector3> positionHistory = new List<Vector3>();

    private float moveTimer;

    private void Start()
    {
        snakeSegments.Add(transform); // Add the head to the segment list
        positionHistory.Add(transform.position); // Track the initial position
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer >= moveInterval)
        {
            moveTimer -= moveInterval;

            // Update the current direction
            currentDirection = nextDirection;

            // Calculate the new position for the head
            Vector3 newPosition = transform.position + (Vector3)(currentDirection * gridSize);

            // Add the new position to the history
            positionHistory.Insert(0, newPosition);

            // Move the head
            transform.position = newPosition;

            // Move each segment to the position of the segment in front of it
            for (int i = 1; i < snakeSegments.Count; i++)
            {
                snakeSegments[i].position = positionHistory[i];
            }

            // Trim the history to match the number of segments
            positionHistory.RemoveAt(positionHistory.Count - 1);
        }
    }

    public void SetInputDirection(Vector2 newDirection)
    {
        if (IsValidDirectionChange(newDirection))
        {
            nextDirection = newDirection;
        }
    }

    private bool IsValidDirectionChange(Vector2 newDirection)
    {
        // Allow only cardinal directions and disallow reversing direction
        return (newDirection == Vector2.up || newDirection == Vector2.down || newDirection == Vector2.left || newDirection == Vector2.right) &&
               (currentDirection + newDirection != Vector2.zero);
    }

    public void Grow()
    {
        // Get the last segment's position
        Transform lastSegment = snakeSegments[snakeSegments.Count - 1];
        Vector3 newSegmentPosition = lastSegment.position;

        // Instantiate a new segment and add it to the list
        GameObject newSegment = Instantiate(snakeBodyPrefab, newSegmentPosition, Quaternion.identity);
        snakeSegments.Add(newSegment.transform);

        // Add the new position to the history (to maintain proper spacing)
        positionHistory.Add(newSegmentPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Snake Body"))
        {
            GameEvents.GameOver();
        }
    }
}
