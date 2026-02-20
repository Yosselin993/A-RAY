using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void Exit(){
        SceneManager.LoadScene("Main Menu");
    }
   
}
