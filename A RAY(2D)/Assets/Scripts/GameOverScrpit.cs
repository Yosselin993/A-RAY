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
        // GameOverPanel.SetActive(false);
        SceneManager.LoadScene("MusicSetup");
    }

    public void Quit_mmm()
    {
        Time.timeScale = 1f; // this unpauses before going to the menu
        GameManager.Instance.ResetAllGameData();
        SongManager.Instance.ResetAllSongData();
        SceneManager.LoadScene("Main Menu");
    }
}
