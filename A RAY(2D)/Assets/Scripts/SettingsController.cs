using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  public void GoBack()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
