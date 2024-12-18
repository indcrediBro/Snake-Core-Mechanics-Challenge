using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMover : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Vector2 spawnAreaMin;
    [SerializeField] private Vector2 spawnAreaMax;
    [SerializeField] private Vector2 min, max;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveInterval = 2f;
    private Vector3 targetPosition;
    private bool moving;

    private void Start()
    {
        SetInitialPosition();
        InvokeRepeating(nameof(MoveToRandomPosition), moveInterval, moveInterval);
    }

    private void SetInitialPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
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
            rigidbody.isKinematic = true;
            Vector2 newPosition = Vector2.MoveTowards(rigidbody.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
            rigidbody.MovePosition(newPosition);

            if (Vector3.Distance(rigidbody.position, targetPosition) < 0.1f)
            {
                rigidbody.isKinematic = false;
                moving = false;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        MoveToRandomPosition();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        MoveToRandomPosition();
    }
}
