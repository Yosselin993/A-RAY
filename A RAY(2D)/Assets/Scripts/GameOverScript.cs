using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public GameObject GameOverPanel;

    void Start()
    {
        GameOverPanel.SetActive(false); // just making sure the panel is hidden when ggame starts
    }

    public void OpenGameoverPanel()
    {
        GameOverPanel.SetActive(true);
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
