using UnityEngine;
using TMPro;
using System.Text;

public class LeaderboardDisplay : MonoBehaviour
{
    [Header("Leaderboard UI")]
    public TMP_Text leaderboardText; // displays the scoreboard

    void OnEnable()
    {
        UpdateLeaderboard(); // when UI is active, refresh the LB display
    }

    public void UpdateLeaderboard()
    {
        if (leaderboardText == null) return; // stop of the text box was not assigned in inspector, safety check
        if (GameManager.Instance == null) return; // stop of the gamemaager does not exist, safty check

        //  using StringBuilder becuase its used to build a longer string
        //  better than using text += text
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("SCOREBOARD");
        sb.AppendLine();

        // If no scores have been saved yet
        if (GameManager.Instance.leaderboard.entries.Count == 0)
        {
            sb.AppendLine("No scores yet.");
        }
        else
        {
            // Show top scores, loops through each save LB and displays
            for (int i = 0; i < GameManager.Instance.leaderboard.entries.Count; i++)
            {
                ScoreEntry entry = GameManager.Instance.leaderboard.entries[i];
                sb.AppendLine((i + 1) + ". " + entry.nickname + " - " + entry.score);
            }
        }

        leaderboardText.text = sb.ToString(); // puts the finished text into the UI
    }
}