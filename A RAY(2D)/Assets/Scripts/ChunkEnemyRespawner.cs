using System.Collections.Generic;
using UnityEngine;

public class ChunkEnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs; // enemies this chunk is allowed to spawn

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

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
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        // this stops if this chunk already spawned enemies once
        if (hasSpawnedEnemies) return;
        hasSpawnedEnemies = true;

        // for safety checks
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        float roll = Random.value;

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
        Transform point = GetRandomUnusedSpawnPoint(new List<Transform>());

        if (point == null) return;

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // this is the parent to chunk so enemy gets removed with chunk
        Instantiate(enemyPrefab, point.position, Quaternion.identity, transform);
    }

    void SpawnEnemyGroup()
    {
        int spawnCount = Random.Range(minGroupSize, maxGroupSize + 1);
        spawnCount = Mathf.Min(spawnCount, spawnPoints.Length);

        List<Transform> usedPoints = new List<Transform>();

        for (int i = 0; i < spawnCount; i++)
        {
            Transform point = GetRandomUnusedSpawnPoint(usedPoints);

            if (point == null) break;

            usedPoints.Add(point);

            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // this is the parent to chunk so enemy gets removed with chunk
            Instantiate(enemyPrefab, point.position, Quaternion.identity, transform);
        }
    }

    Transform GetRandomUnusedSpawnPoint(List<Transform> usedPoints)
    {
        List<Transform> availablePoints = new List<Transform>();

        foreach (Transform point in spawnPoints)
        {
            if (!usedPoints.Contains(point))
            {
                availablePoints.Add(point);
            }
        }

        if (availablePoints.Count == 0) return null;

        return availablePoints[Random.Range(0, availablePoints.Count)];
    }
}