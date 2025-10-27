using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public List<Food> foodPrefabs;
    public Transform foodParent;
    public float spawnRate = 2f;
    public float spawnRange = 10f;

    private float timer;

    private void Start()
    {
        timer = 0f;



        for (int i = 0; i < 50; i++)
        {
            SpawnFood();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnRate)
        {
            SpawnFood();
            timer = 0f;
        }
    }

    public GameObject GetFoodPrefab() => foodPrefabs[UnityEngine.Random.Range(0,foodPrefabs.Count)].gameObject;
    private void SpawnFood()
    {
        Vector3 spawnLocation = transform.position + (Vector3)Random.insideUnitCircle * spawnRange;
        Instantiate(GetFoodPrefab(), spawnLocation, Quaternion.identity, foodParent);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}
