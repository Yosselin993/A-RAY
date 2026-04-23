using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;    // reference to the player
    public GameObject chunkPrefab; // chunk prefab to repeat

    [Header("Chunk Settings")]
    public float chunkWidth = 20f; // width of one chunk///testing the actual size is 17.97
    public int chunkVisibleAroundPlayer = 2; // Number of chunks kept on each side of the player

    private Dictionary<int, GameObject> spawnedChunks = new Dictionary<int, GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateChunks();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateChunks();
    }

    void UpdateChunks()
    {
        int playerChunkIndex = Mathf.FloorToInt(player.position.x / chunkWidth); // Finds which chunk the player is currently inside

        // spawn needed chunkd around the player
        for (int i = playerChunkIndex - chunkVisibleAroundPlayer; i <= playerChunkIndex + chunkVisibleAroundPlayer; i++)
        {
            if (!spawnedChunks.ContainsKey(i))
            {
                SpawnChunk(i);
            }
        }
        List<int> chunksToRemove = new List<int>(); // stores chunks that should be removed

        foreach (KeyValuePair<int, GameObject> chunk in spawnedChunks)
        {
            if (Mathf.Abs(chunk.Key - playerChunkIndex) > chunkVisibleAroundPlayer)
            {
                chunksToRemove.Add(chunk.Key);
            }
        }
        // removes old chunks
        foreach (int chunkIndex in chunksToRemove)
        {
            Destroy(spawnedChunks[chunkIndex]);
            spawnedChunks.Remove(chunkIndex);
        }
    }
    void SpawnChunk(int chunkIndex)
    {
        Vector3 spawnPosition = new Vector3(chunkIndex * chunkWidth, 0f, 0f);
        GameObject newChunk = Instantiate(chunkPrefab, spawnPosition, Quaternion.identity);
        spawnedChunks.Add(chunkIndex, newChunk);
    }
}
