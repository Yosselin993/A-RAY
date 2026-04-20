using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{


    [System.Serializable]
    public class background
    {
        public string genre;
        public Sprite backgroundSprites;
    }

    public Image backgroundImage;
    public background[] genreBackgrounds;
    public Sprite defultBackground;


    public void ChangeBackground(string genre)
    {

        if (string.IsNullOrEmpty(genre))
        {
            setDefultBackground();
            return;
        }

        string cleanGenre = genre.ToLower().Trim();

        foreach (background item in genreBackgrounds)
        {
            if (cleanGenre.Contains(item.genre.ToLower()))
            {
                backgroundImage.sprite = item.backgroundSprites;
                Debug.Log("The background has changed for genre " + genre);
                return;
            }
        }
        setDefultBackground();

    }

    void setDefultBackground()
    {
        backgroundImage.sprite = defultBackground;
        Debug.Log("No genre was given so defult background was applied");
    }



   
}
