using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{


    [System.Serializable]
    public class background
    {
        public string genre;
        // public Sprite backgroundSprites;
        //change to fix the prefab backgrounds
        public GameObject backgroundPrefabs;
    }

    // public Image backgroundImage;
    public Transform backgroundspawn;
    public background[] genreBackgrounds;
    public GameObject defaultBackground;
     private GameObject currentBackground;


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
                SpawnBackground(item.backgroundPrefabs);
                Debug.Log("The background has changed for genre " + genre);
                return;
            }
        }
        setDefultBackground();

    }

    void SpawnBackground(GameObject prefab)
    {
        Debug.Log("Spawning prefab: " + prefab.name);
        if (prefab == null)
        {
            Debug.LogWarning("Prefab is null.");
            return;
        }

        // remove old background
        if (currentBackground != null)
        {
            Destroy(currentBackground);
        }

        // create new one
        currentBackground = Instantiate(prefab, backgroundspawn);

        currentBackground.transform.localPosition = Vector3.zero;
        currentBackground.transform.localRotation = Quaternion.identity;
        currentBackground.transform.localScale = Vector3.one;
    }




    void setDefultBackground()
    {
        SpawnBackground(defaultBackground);
        Debug.Log("No genre was given so defult background was applied");
    }



   
}
