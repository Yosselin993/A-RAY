using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public GameObject settingPanel;
    public GameObject difficultyPanel;
    private Difficulty selectedDifficulty;

    public GameObject morePanel; // for 'More' Panel 

    public TMP_InputField nicknameInput; // would drag TMP input field here

     void Start()
    {
        selectedDifficulty = GameManager.Instance.currentDifficulty;

        // shows when opening 
        if (nicknameInput != null && GameManager.Instance != null)
        {
            nicknameInput.text = GameManager.Instance.currentNickname;
        }
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

    public void OpenMusicScene()
    {
        SceneManager.LoadScene("MusicSetup");
    }

    public void OpenMorePanel()
    {
        morePanel.SetActive(true);
    }

    public void CloseMorePanel()
    {
        morePanel.SetActive(false);
    }

    // Called by TMP input field to save the player's nickname
    public void SetPlayerNickname(string nickname)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetNickname(nickname);
            Debug.Log("Nickname set to: " + GameManager.Instance.currentNickname); // debugging
        }
    }

    // used to reset score before starting game
    public void PlayGame()
    {
        // forces to grab nickname from input field before starting
        if (nicknameInput != null)
        {
            SetPlayerNickname(nicknameInput.text);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetScore();
        }

        SceneManager.LoadScene("PrototypeScene");
    }

   
}
