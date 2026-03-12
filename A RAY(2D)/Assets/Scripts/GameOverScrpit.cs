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
        Time.timeScale = 0f;
    }
    public void Restart_mm()
    {
        // GameOverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit_mmm()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
