using UnityEngine;
using UnityEngine.SceneManagement;
public class MoreController : MonoBehaviour
{
    
    public void GoBack()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
