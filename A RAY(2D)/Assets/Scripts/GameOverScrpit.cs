using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScrpit : MonoBehaviour
{
   public GameObject GameOverPanel;

    void Start()
    {
        GameOverPanel.SetActive(false); // just making sure the panel is hidden when ggame starts
    }

    public void OpenGameoverPanel()
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0f; // this pauses the game
    }
    public void Restart_mm()
    {
        Time.timeScale = 1f; // this unpauses before reloading
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetScore();
        }

        // Send label BEFORE loading the next scene for Data Collection
        var dc = FindFirstObjectByType<DataCollector>();
        if (dc != null){
            dc.RecordFinalLabel(0);   // or 1
        }
        
        // GameOverPanel.SetActive(false);
        SceneManager.LoadScene("MusicSetup");
    }

    public void Quit_mmm()
    {
        Time.timeScale = 1f; // this unpauses before going to the menu
        GameManager.Instance.ResetAllGameData();
        SongManager.Instance.ResetAllSongData();

        // Send label BEFORE loading the next scene
        var dc = FindFirstObjectByType<DataCollector>();
        if (dc != null){
            dc.RecordFinalLabel(0);   // or 1
        }

        SceneManager.LoadScene("Main Menu");
    }
}
