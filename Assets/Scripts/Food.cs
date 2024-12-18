using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Snake"))
        {
            GameEvents.FoodEaten();
            Destroy(gameObject);
        }
    }
}
