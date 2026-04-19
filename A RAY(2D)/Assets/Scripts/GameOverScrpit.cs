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

        // GameOverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit_mmm()
    {
        Time.timeScale = 1f; // this unpauses before going to the menu
        SceneManager.LoadScene("Main Menu");
    }
}
