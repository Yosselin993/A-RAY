using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("PrototypeScene");
    }

    public void OpenMore()
    {
        SceneManager.LoadScene("More Menu");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings Menu");
    }
   
}
