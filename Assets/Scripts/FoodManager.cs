using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private Vector2 spawnAreaMin;
    [SerializeField] private Vector2 spawnAreaMax;

    private void Start()
    {
        SpawnFood();
        GameEvents.OnFoodEaten += SpawnFood;
    }

    private void OnDestroy()
    {
        GameEvents.OnFoodEaten -= SpawnFood;
    }

    private void SpawnFood()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            0
        );
        Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }
}
