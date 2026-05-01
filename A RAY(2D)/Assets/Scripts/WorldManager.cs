using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;    // reference to the player
    public GameObject chunkPrefab; // chunk prefab to repeat, this is to create our endless walk

    [Header("Chunk Settings")]
    public float chunkWidth = 20f; // width of one chunk///testing the actual size is 17.97
    public int chunkVisibleAroundPlayer = 2; // Number of chunks kept on each side of the player

    // Dictionary sotres spawned chunks using their chunk index, int = chunk number, gameobject = chunk object in the scene
    private Dictionary<int, GameObject> spawnedChunks = new Dictionary<int, GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateChunks(); // this will spawn the starting chunks as the game begins
    }

    // Update is called once per frame
    void Update()
    {
        UpdateChunks(); // this will keep checking if new chunks need to be spawned 
    }

    void UpdateChunks()
    {
        int playerChunkIndex = Mathf.FloorToInt(player.position.x / chunkWidth); // Finds which chunk the player is currently inside

        // spawn needed chunkd around the player
        for (int i = playerChunkIndex - chunkVisibleAroundPlayer; i <= playerChunkIndex + chunkVisibleAroundPlayer; i++)
        {
            // only spawn the chunk if it does not already exist
            if (!spawnedChunks.ContainsKey(i))
            {
                SpawnChunk(i);
            }
        }
        // stores chunks that are too far away
        List<int> chunksToRemove = new List<int>(); // stores chunks that should be removed

        foreach (KeyValuePair<int, GameObject> chunk in spawnedChunks)
        {
            // if the chunk is father than the visible range, mark for removal
            if (Mathf.Abs(chunk.Key - playerChunkIndex) > chunkVisibleAroundPlayer)
            {
                chunksToRemove.Add(chunk.Key);
            }
        }
        // removes old chunks
        // we do this after because changing a Dictionary while looping through it causes errors
        foreach (int chunkIndex in chunksToRemove)
        {
            Destroy(spawnedChunks[chunkIndex]);
            spawnedChunks.Remove(chunkIndex);
        }
    }
    void SpawnChunk(int chunkIndex)
    {
        // we are calculating where the chunk should appear
        Vector3 spawnPosition = new Vector3(chunkIndex * chunkWidth, 0f, 0f);
        GameObject newChunk = Instantiate(chunkPrefab, spawnPosition, Quaternion.identity); // this creates the chunk in the scene
        spawnedChunks.Add(chunkIndex, newChunk); // this will save it into the Dictionary so we know this chunk already exists
    }
}
