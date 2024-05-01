using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject fuel;

    private float zEnemySpawn = 50.0f;
    //private float xSpawnRange = 100.0f;
    private float zFuelRange = 12.0f;

    private float fuelSpawnTime = 5.0f;
    private float enemySpawnTime = 1.0f;
    private float startDelay = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", startDelay, enemySpawnTime);
        InvokeRepeating("SpawnFuel", startDelay, fuelSpawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy()
    {
        float randomX = Random.Range(350, 450);
        int randomIndex = Random.Range(0, enemies.Length);

        Vector3 spawnPos = new Vector3(randomX, 1, zEnemySpawn);

        Instantiate(enemies[randomIndex], spawnPos, enemies[randomIndex].gameObject.transform.rotation);
    }

    void SpawnFuel()
    {
        float randomX = Random.Range(300, 400);
        float randomZ = Random.Range(-zFuelRange, zFuelRange);

        Vector3 spawnPos = new Vector3(randomX, 1, randomZ);
        Instantiate(fuel, spawnPos, fuel.gameObject.transform.rotation);

    }
}
