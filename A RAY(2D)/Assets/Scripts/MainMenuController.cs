using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject settingPanel;
    public GameObject difficultyPanel;
    private Difficulty selectedDifficulty;

     void Start()
    {
        selectedDifficulty = GameManager.Instance.currentDifficulty;
    }
    public void Exit(){
       SceneManager.LoadScene("Main Menu");
       //Application.Quit();
    }

      public void OpenDifficulty()
    {
        settingPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void GOBack()
    {
        settingPanel.SetActive(true);
        difficultyPanel.SetActive(false);
    }

    public void SelectEasy()
    {
        selectedDifficulty = Difficulty.Easy;
    }

    public void SelectMedium()
    {
        selectedDifficulty = Difficulty.Medium;
    }

    public void SelectHard()
    {
        selectedDifficulty = Difficulty.Hard;
    }

    public void ApplyDifficulty()
    {
        GameManager.Instance.currentDifficulty = selectedDifficulty;
        GOBack();
    }

   
}
