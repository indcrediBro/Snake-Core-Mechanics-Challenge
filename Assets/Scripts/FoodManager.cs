using UnityEngine;
using System.Collections.Generic;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private GameObject[] foodPrefabs;
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private GameObject[] powerUpPrefabs; // Array of power-up prefabs
    [SerializeField] private Vector2 spawnAreaMin;
    [SerializeField] private Vector2 spawnAreaMax;

    private List<GameObject> spawnedObstacles = new List<GameObject>();
    private GameObject currentTarget;

    private void OnEnable()
    {
        GameEvents.OnGameStart += ResetSpawns;
        GameEvents.OnFoodEaten += SpawnFoodAndObstacle;
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStart -= ResetSpawns;
        GameEvents.OnFoodEaten -= SpawnFoodAndObstacle;
    }

    private void ResetSpawns()
    {
        if (spawnedObstacles.Count > 0)
        {
            foreach (GameObject item in spawnedObstacles)
            {
                Destroy(item);
            }
            spawnedObstacles = new List<GameObject>();
        }

        if (currentTarget)
        {
            Destroy(currentTarget);
        }
        SpawnFood();
    }

    private void SpawnFood()
    {
        currentTarget = SpawnItem(foodPrefabs[Random.Range(0, foodPrefabs.Length)], "Food");
    }

    private void SpawnFoodAndObstacle()
    {

        if (Random.value < 0.2f) // 20% chance to spawn a power-up
        {
            GameObject powerUp = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
            currentTarget = SpawnItem(powerUp, "PowerUp");
        }
        else
        {
            SpawnFood();
        }

        spawnedObstacles.Add(SpawnItem(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], "Obstacle"));
    }

    private GameObject SpawnItem(GameObject prefab, string tag)
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            0
        );

        GameObject item = Instantiate(prefab, spawnPosition, Quaternion.identity);
        item.tag = tag;

        return item;
    }
}
