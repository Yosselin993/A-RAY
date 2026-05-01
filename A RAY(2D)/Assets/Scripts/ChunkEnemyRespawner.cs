using System.Collections.Generic;
using UnityEngine;

public class ChunkEnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs; // enemies this chunk is allowed to spawn

    [Header("Spawn Points")]
    public Transform[] spawnPoints; // enemy spawn points 

    [Header("Spawn Chances")]
    [Range(0f, 1f)] public float noEnemyChance = 0.25f;   // 25% no enemies
    [Range(0f, 1f)] public float singleEnemyChance = 0.50f; // 50% one enemy
    [Range(0f, 1f)] public float groupEnemyChance = 0.25f;  // 25% group

    [Header("Group Settings")]
    public int minGroupSize = 2;
    public int maxGroupSize = 3;

    private bool hasSpawnedEnemies = false; 

    void Start()
    {
        SpawnEnemies(); // this will spawns enemies as soon as chunck appears
    }

    public void SpawnEnemies()
    {
        // this stops if this chunk already spawned enemies once
        if (hasSpawnedEnemies) return;
        hasSpawnedEnemies = true;

        // for safety checks, if no enemy prefabs or spawn points are assigned, do nothing
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        float roll = Random.value; // random.value gives a random decimal between 0 and 1

        // if no enemies in this chunk
        if (roll < noEnemyChance)
        {
            return;
        }

        // if one enemy in this chunk
        if (roll < noEnemyChance + singleEnemyChance)
        {
            SpawnSingleEnemy();
            return;
        }

        // otherwise spawn a small group
        SpawnEnemyGroup();
    }

    void SpawnSingleEnemy()
    {
        // one random spawn point
        Transform point = GetRandomUnusedSpawnPoint(new List<Transform>());

        if (point == null) return;

        // a random enemy prefab from enemy prefab array
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // this is the parent to chunk so enemy gets removed with chunk
        Instantiate(enemyPrefab, point.position, Quaternion.identity, transform);
    }

    void SpawnEnemyGroup()
    {
        // picks a random group size between minGroupSize and maxGroupSize
        int spawnCount = Random.Range(minGroupSize, maxGroupSize + 1);
        spawnCount = Mathf.Min(spawnCount, spawnPoints.Length); // making sure enemies dont spawn more than available spawn points

        List<Transform> usedPoints = new List<Transform>(); // this keeps track of which spawn points were already used

        for (int i = 0; i < spawnCount; i++)
        {
            // this gets a random spawn point that has not been used 
            Transform point = GetRandomUnusedSpawnPoint(usedPoints);

            if (point == null) break;

            usedPoints.Add(point); // marks this point as used, so enemys dont spawn in the same spot

            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]; // picking random enemy prefab

            // this is the parent to chunk so enemy gets removed with chunk
            Instantiate(enemyPrefab, point.position, Quaternion.identity, transform);
        }
    }

    Transform GetRandomUnusedSpawnPoint(List<Transform> usedPoints)
    {
        List<Transform> availablePoints = new List<Transform>(); // list of spawn points available

        foreach (Transform point in spawnPoints)
        {
            // only adds the spawn points that have not been used
            if (!usedPoints.Contains(point))
            {
                availablePoints.Add(point);
            }
        }

        // if there is no available points left, return nothing
        if (availablePoints.Count == 0) return null;
        // return one random avaliable spawn point
        return availablePoints[Random.Range(0, availablePoints.Count)];
    }
}