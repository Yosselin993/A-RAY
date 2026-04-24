using UnityEngine;
using UnityEngine.UI;


public class DiffcuityDisplay : MonoBehaviour
{
    // gamemodes
    public Image iconDisplay;
    public Sprite easyIcon;
    public Sprite mediumIcon;
    public Sprite hardIcon;

    void OnEnable()
    {
        //menu will show difficulty when opened
        if (GameManager.Instance != null)
            Show(GameManager.Instance.currentDifficulty);
    }
    // basically this script focuses on showing the difficulty of the game at the moment
    // made so its easier to know
    public void ShowEasy()
    {
        Show(Difficulty.Easy);
    }
    public void ShowMedium()
    {
        Show(Difficulty.Medium);
    }
    public void ShowHard()
    {
        Show(Difficulty.Hard);
    }

    void Show(Difficulty current)
    {
        if (iconDisplay == null) return;

        switch (current)
        {
            case Difficulty.Easy:
                iconDisplay.sprite = easyIcon;
                break;

            case Difficulty.Medium:
                iconDisplay.sprite = mediumIcon;
                break;

            case Difficulty.Hard:
                iconDisplay.sprite = hardIcon;
                break;
        }
    }
}
