using UnityEngine;
using System.Collections.Generic;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

// One saved leaderboard entry
[System.Serializable] // this allows unity to convert this into JSON
public class ScoreEntry
{
    public string nickname; // player name
    public int score; // player score

    public ScoreEntry(string nickname, int score) // to create a new score entry
    {
        this.nickname = nickname;
        this.score = score;
    }
}

// Wrapper so Unity can save/load the list as JSON
// basically wraps a list of score entry object
// Since unity JsonUtility doesnt save/load a raw list good by itself, we place the list inside a class
[System.Serializable]
public class LeaderboardData
{
    public List<ScoreEntry> entries = new List<ScoreEntry>();
}


public class GameManager : MonoBehaviour
{
   public static GameManager Instance; // lets other scripts access the GameManger 

    [Header("Difficulty")]
    public Difficulty currentDifficulty = Difficulty.Medium;

    [Header("Current Run")]
    public string currentNickname = "Player"; // name entered by player
    public int currentScore = 0; // score during current gameplay

    [Header("Leaderboard")]
    public LeaderboardData leaderboard = new LeaderboardData(); // prevents saving the same run mutiple times

    // Prevents saving the same run more than once
    public bool runSaved = false;

    private void Awake()
    {
        // Singleton setup
        // if no instance exist -> this becomes the main one
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // keeps the gameobject alive when switching scenes

            // Load saved leaderboard when game starts
            LoadLeaderboard();
        }
        else
        {
            Destroy(gameObject); // if another gamemanager exists, destroy this one 
        }
    }

    // Save/update the current player's nickname
    public void SetNickname(string nickname)
    {
        // if empty -> default name
        if (string.IsNullOrWhiteSpace(nickname))
        {
            currentNickname = "Player";
        }
        else
        {
            // trim removes exrta spaces
            currentNickname = nickname.Trim();
        }
    }

    // Add points during gameplay
    public void AddScore(int amount)
    {
        currentScore += amount;
    }

    // Reset score for a new run
    public void ResetScore()
    {
        currentScore = 0;
        runSaved = false; // allow saving again for the next run
    }

    // Save the current finished run into the leaderboard
    public void SaveCurrentRunToLeaderboard()
    {
        // Do not save twice, prevents dups
        if (runSaved) return;

        ScoreEntry newEntry = new ScoreEntry(currentNickname, currentScore); // creates a new entry with current data
        leaderboard.entries.Add(newEntry); // adds it to the list

        // Sort highest to lowest
        leaderboard.entries.Sort((a, b) => b.score.CompareTo(a.score));

        // Keep only top 5 scores
        if (leaderboard.entries.Count > 5)
        {
            leaderboard.entries.RemoveRange(5, leaderboard.entries.Count - 5); // keeps only top 5 scores
        }

        SaveLeaderboard(); // saves updated leaderboard to disk, perm.
        runSaved = true; // marks as saved so it doesnt save again
    }

    //Win + Data Collector
    public int EvaluateRunAndReturnLabel(bool survivedPlaylist)
    {
        // 1. Calculate final score
        int finalScore = currentScore;

        if (survivedPlaylist)
        {
            finalScore += 200; // survival bonus
        }

        // 2. Determine leaderboard threshold
        int threshold = 0;

        if (leaderboard.entries.Count >= 5)
        {
            threshold = leaderboard.entries[leaderboard.entries.Count - 1].score;
        }

        // 3. Compute label
        int label = finalScore >= threshold ? 1 : 0;

        // 4. Update currentScore so SaveCurrentRunToLeaderboard saves the correct value
        currentScore = finalScore;

        // 5. Save to leaderboard
        SaveCurrentRunToLeaderboard();

        // 6. Return label so DataCollector can log it
        return label;
    }

    // Save leaderboard as JSON into PlayerPrefs
    // We are using JSON because unity can save strings easily with PlayerPrefs, so that the leaderboard object
    // is converted into JSON text, saved, then converted back into an object when the game loads again.
    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboard); // converts LB object into JSON string
        PlayerPrefs.SetString("Leaderboard", json); // save JSON string int PlayerPrefs, Unitys simple storage system
        PlayerPrefs.Save(); // write data to disk
    }

    // Load leaderboard from PlayerPrefs
    private void LoadLeaderboard()
    {
        if (PlayerPrefs.HasKey("Leaderboard")) // if LB exist
        {
            string json = PlayerPrefs.GetString("Leaderboard"); // get saved JSON string text from PlayerPrefs
            leaderboard = JsonUtility.FromJson<LeaderboardData>(json); // convert JSON back into LB object

            // safty check, seeing if anything went wrong
            if (leaderboard == null)
            {
                leaderboard = new LeaderboardData();
            }
        }
        else
        {
            // if no data, create new LB
            leaderboard = new LeaderboardData();
        }
    }

    public void ResetAllGameData()
    {
        // Clear score
        currentScore = 0;
        currentNickname = "Player";
    }
}
