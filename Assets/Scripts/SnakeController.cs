using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private float moveInterval = 0.2f;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private GameObject snakeBodyPrefab;

    private Vector2 currentDirection = Vector2.zero;
    private Vector2 nextDirection = Vector2.right;
    private List<Transform> snakeSegments = new List<Transform>();
    private List<Vector3> positionHistory = new List<Vector3>();

    private float originalSpeed;
    private float moveTimer;
    private bool isSpeedBoosted = false;
    private bool isSlowMotionActive = false;
    private bool areControlsReversed = false;

    private void OnEnable()
    {
        GameEvents.OnGameStart += ResetSnake;
        GameEvents.OnFoodEaten += Grow;
        originalSpeed = moveInterval;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= ResetSnake;
        GameEvents.OnFoodEaten -= Grow;
    }

    private void Update()
    {
        HandleMovement();
    }

    public void ResetSnake()
    {
        transform.position = Vector3.zero;
        currentDirection = Vector2.zero;
        StopAllCoroutines();
        isSpeedBoosted = false;
        isSlowMotionActive = false;
        areControlsReversed = false;
        moveInterval = originalSpeed;

        if (!snakeSegments.Contains(transform))
            snakeSegments.Add(transform);

        for (int i = snakeSegments.Count - 1; i > 0; i--)
        {
            Transform segment = snakeSegments[i];
            snakeSegments.RemoveAt(i);
            Destroy(segment.gameObject);
        }

        positionHistory.Clear();
        positionHistory.Add(transform.position);
        Debug.Log("Resetting Snake!");
    }

    private void HandleMovement()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer >= moveInterval)
        {
            moveTimer -= moveInterval;

            currentDirection = nextDirection;
            Vector3 newPosition = transform.position + (Vector3)(currentDirection * gridSize);
            positionHistory.Insert(0, newPosition);
            transform.position = newPosition;

            for (int i = 1; i < snakeSegments.Count; i++)
            {
                if (i < positionHistory.Count)
                {
                    snakeSegments[i].position = positionHistory[i];
                }
            }

            positionHistory.RemoveAt(positionHistory.Count - 1);
        }
    }

    public void SetInputDirection(Vector2 newDirection)
    {
        if (areControlsReversed)
        {
            newDirection = -newDirection; // Reverse the controls
        }

        if (IsValidDirectionChange(newDirection))
        {
            nextDirection = newDirection;
        }
    }

    private bool IsValidDirectionChange(Vector2 newDirection)
    {
        return (newDirection == Vector2.up || newDirection == Vector2.down || newDirection == Vector2.left || newDirection == Vector2.right) &&
               (currentDirection + newDirection != Vector2.zero);
    }

    public void Grow()
    {
        Transform lastSegment = snakeSegments[snakeSegments.Count - 1];
        Vector3 newSegmentPosition = lastSegment.position;

        GameObject newSegment = Instantiate(snakeBodyPrefab, newSegmentPosition, Quaternion.identity);
        if (snakeSegments.Count > 2) newSegment.tag = "Snake Body";
        snakeSegments.Add(newSegment.transform);

        positionHistory.Add(newSegmentPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Snake Body"))
        {
            GameEvents.GameOver();
        }
    }

    private void HandleFoodEaten(GameObject food)
    {
        if (food.CompareTag("Food"))
        {
            Grow();
        }
        else if (food.CompareTag("PowerUp"))
        {
            //ActivatePowerUp(food.GetComponent<PowerUp>().type);
            Grow();
        }
    }

    public void ActivatePowerUp(PowerUpType type)
    {
        Debug.Log(type);
        switch (type)
        {
            case PowerUpType.SpeedBoost:
                StartCoroutine(SpeedBoost());
                break;
            case PowerUpType.SlowMotion:
                StartCoroutine(SlowMotion());
                break;
            case PowerUpType.ReverseControls:
                StartCoroutine(ReverseControls());
                break;
        }
    }

    private IEnumerator SpeedBoost()
    {
        AudioManager.Instance.PlaySound("PowerUp");
        isSpeedBoosted = true;
        moveInterval /= 2;
        yield return new WaitForSeconds(8);
        moveInterval *= 2;
        isSpeedBoosted = false;
        AudioManager.Instance.PlaySound("PowerDown");
    }

    private IEnumerator SlowMotion()
    {
        AudioManager.Instance.PlaySound("PowerUp");
        isSlowMotionActive = true;
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(5);
        Time.timeScale = 1.0f;
        isSlowMotionActive = false;
        AudioManager.Instance.PlaySound("PowerDown");
    }

    private IEnumerator ReverseControls()
    {
        AudioManager.Instance.PlaySound("PowerUp");
        areControlsReversed = true;
        yield return new WaitForSeconds(5);
        areControlsReversed = false;
        AudioManager.Instance.PlaySound("PowerDown");
    }
}
