using UnityEngine;
using System.Collections.Generic;

//Need new object to collect this scipt to
public class DataCollector : MonoBehaviour
{
    public float sampleInterval = 15f;
    private float timer = 0f;

    private List<string> rows = new List<string>();
    private bool runEnded = false;

    // For computing rates
    private int lastScore = 0;
    private int lastHealth = 0;
    private float timeElapsed = 0f;

    void Start()
    {
        rows.Add("currentScore,currentHealth,playlistProgress,songsRemaining,averageScoreRate,healthDecayRate,leaderboardThreshold,timeElapsed,label");

        lastScore = GameManager.Instance.currentScore;
        lastHealth = FindFirstObjectByType<PlayerHealth>().currentHealth;
    }

    void Update()
    {
        if (runEnded) return;

        timer += Time.deltaTime;
        timeElapsed += Time.deltaTime;

        if (timer >= sampleInterval)
        {
            timer = 0f;
            RecordSample();
        }
    }

    void RecordSample()
    {
        int score = GameManager.Instance.currentScore;
        int health = FindFirstObjectByType<PlayerHealth>().currentHealth;

        // Compute score rate
        float avgScoreRate = (score - lastScore) / sampleInterval;
        lastScore = score;

        // Compute health decay rate
        float healthDecayRate = (health - lastHealth) / sampleInterval;
        lastHealth = health;

        // Playlist progress + songs remaining from MusicPlayer
        MusicPlayer mp = FindFirstObjectByType<MusicPlayer>();
        float playlistProgress = 0f;
        int songsRemaining = 0;

        if (mp != null)
        {
            int index = mp.GetCurrentIndex();
            int total = mp.GetTotalSongs();

            if (total > 0)
            {
                playlistProgress = (float)index / total;
                songsRemaining = total - index - 1;
            }
        }

        int threshold = GetLeaderboardThreshold();

        string row = $"{score},{health},{playlistProgress},{songsRemaining},{avgScoreRate},{healthDecayRate},{threshold},{timeElapsed},";
        rows.Add(row);

        Debug.Log("[DataCollector] Sample: " + row);
    }

    public float GetPlaylistProgress()
    {
        MusicPlayer mp = FindFirstObjectByType<MusicPlayer>();
        if (mp == null) return 0f;

        int index = mp.GetCurrentIndex();
        int total = mp.GetTotalSongs();
        if (total == 0) return 0f;

        return (float)index / total;
    }

    public int GetSongsRemaining()
    {
        MusicPlayer mp = FindFirstObjectByType<MusicPlayer>();
        if (mp == null) return 0;

        int index = mp.GetCurrentIndex();
        int total = mp.GetTotalSongs();
        return Mathf.Max(0, total - index - 1);
    }

    public float GetAverageScoreRate()
    {
        return (GameManager.Instance.currentScore - lastScore) / sampleInterval;
    }

    public float GetHealthDecayRate()
    {
        return (FindFirstObjectByType<PlayerHealth>().currentHealth - lastHealth) / sampleInterval;
    }
    public int GetLeaderboardThreshold()
    {
        var lb = GameManager.Instance.leaderboard.entries;

        if (lb.Count < 5)
            return 0;

        return lb[lb.Count - 1].score;
    }
    public float GetTimeElapsed()
    {
        return timeElapsed;
    }

    public void RecordFinalLabel(int label)
    {
        runEnded = true;

        string finalRow = $",,,,,,,,{label}";
        rows.Add(finalRow);

        Debug.Log("=== DATA COLLECTION OUTPUT START ===");
        foreach (var r in rows)
            Debug.Log(r);
        Debug.Log("=== DATA COLLECTION OUTPUT END ===");
    }

    void OnDestroy()
    {
        Debug.Log("DATACOLLECTOR DESTROYED");
    }
}