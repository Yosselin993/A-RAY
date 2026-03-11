using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public GameObject GameOverPanel;

    public void OpenGameoverPanel()
    {
        GameOverPanel.SetActive(true);
    }
    public void Restart_mm()
    {
        GameOverPanel.SetActive(false);
    }

    public void Quit_mmm()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
