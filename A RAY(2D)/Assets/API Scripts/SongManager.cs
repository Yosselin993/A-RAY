using System.Collections.Generic;
using UnityEngine;
using TMPro;


//I made this class so we can group song data together that way we dont mix things around. Also, it just looks cleaner 
[System.Serializable]
public class DownloadedSong
{
    public string path; //stores the loacl file path of the downloaded song
    public string genre; //stores the genre of the song that was downloaded 


    public DownloadedSong(string path, string genre) //The constructor of the class.
    {
        this.path = path;
        this.genre = genre;
    }
}



public class SongManager : MonoBehaviour
{
    public static SongManager Instance;

    // public TMP_Text storedSongsText;

    public Transform storedSongsContainer;
    public GameObject storedSongPrefab;



    private SongSuggestion selectedSong;

    //public List<string> downloadedSongPaths = new List<string>();
    //it stores the downloaded songs as a object instead of just a file path.
    //will no longer only contain the file path but also the genre together 
    public List<DownloadedSong> downloadedSongs = new List<DownloadedSong>();

    private List<SongSuggestion> storedSongs = new List<SongSuggestion>();
    // Track downloaded songs by video_id
    public Dictionary<string, string> downloadedById = new Dictionary<string, string>();
    //this will store the genre for each song using its voideo ID
    //this way we can lookup the genre quicker without searching through the list.
    public Dictionary<string, string> genreById = new Dictionary<string, string>();

    public int maxsonglist = 8;
    public int minsonglist = 5;
    public TMP_InputField searchInput; //this will hold the textbox in the inspector field 

    public List<SongSuggestion> GetStoredSongs()
    {
        return storedSongs;
    }




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[SongManager] Created new persistent instance");
        }
        if(Instance != this){
            Debug.LogWarning("[SongManager] Duplicate instance detected — destroying this one");
            Destroy(gameObject);
        }
    }

    //this will be called once the player clicks on a suggested song.
    public void SetSelectedSong(SongSuggestion song)
    {
        selectedSong = song;
        Debug.Log("[SongManager] Selected song set to: " + song.title + " - " + song.artist);
    }

    // public void AddSelectedSong()
    // {
    //     if (selectedSong == null)
    //     {
    //         Debug.Log("no song was selected");
    //         return;
    //     }

    //     storedSongs.Add(selectedSong);

    //     storedSongsText.text = "";
    //     foreach (var song in storedSongs)
    //     {
    //         storedSongsText.text += song.title + " - " + song.artist  + "\n";
    //     }

    //     selectedSong = null;
    // }

    public void AddSelectedSong()
    {
        // FIX: reconnect searchInput if it was lost after scene reload
        if (searchInput == null)
            searchInput = FindFirstObjectByType<TMP_InputField>();
        Debug.Log("[SongManager] AddSelectedSong() called");

        //checking if the textbox is empty(added a new variable called searchInput that holds the textbox in inspector).
        if (string.IsNullOrWhiteSpace(searchInput.text))
        {
            Debug.LogWarning("[SongManager]Since textbox is empty. No song or empty box will be added to StoaredSongPanel");
            selectedSong = null; //this will make sure the last song typed in the textbox is not remembered. So when you click on the add button with no text. Nothing SHOULD BE ADDED.
            return;
        }

        if (selectedSong == null)
        {
            Debug.LogWarning("[SongManager] selectedSong is NULL — user clicked Add before selecting a suggestion");
            return;
        }
        Debug.Log("[SongManager] Current storedSongs count BEFORE adding: " + storedSongs.Count);
        if (storedSongs.Count >= maxsonglist)
        {
            Debug.Log("Can only select up to 8 songs");
            return;
        }

        storedSongs.Add(selectedSong);
        Debug.Log("[SongManager] Added song: " + selectedSong.title + " - " + selectedSong.artist);
        Debug.Log("[SongManager] storedSongs count AFTER adding: " + storedSongs.Count);
        RefreshStoredSongsUI();
        //selectedSong = null; // with this removed you can now click on the same song and keep adding it untill you search for another
    }

    // void RefreshStoredSongsUI()
    // {
    //     foreach (Transform child in storedSongsContainer)
    //     {
    //         Destroy(child.gameObject);
    //     }

    //     foreach (var song in storedSongs)
    //     {
    //         GameObject obj = Instantiate(storedSongPrefab, storedSongsContainer);
    //         TMP_Text textComponent = obj.GetComponentInChildren<TMP_Text>();

    //         if (textComponent != null)
    //         {
    //             textComponent.text = song.title + " - " + song.artist;
    //         }
    //     }
    // }



    public void RefreshStoredSongsUI()
    {
        if (storedSongsContainer == null)
        {
            var go = GameObject.Find("StoredPenal");
            if (go == null)
            {
                Debug.LogError("[SongManager] Could not find StoredSongsContainer in scene");
                return;
            }
            storedSongsContainer = go.transform;
        }
        Debug.Log("[SongManager] RefreshStoredSongsUI() — rebuilding UI with " + storedSongs.Count + " songs");
        foreach (Transform child in storedSongsContainer)
        {
            Destroy(child.gameObject);
        }

      foreach (var song in storedSongs)
      {
            Debug.Log("[SongManager] Creating UI row for: " + song.title);
            GameObject obj = Instantiate(storedSongPrefab, storedSongsContainer);

            TMP_Text textComponent = obj.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = song.title + " - " + song.artist;
            }

            StoredSongPrefabScript row = obj.GetComponent<StoredSongPrefabScript>();
            if (row != null)
            {
                row.Setup(song);
            }
      }

    }

   public void DeleteSong(SongSuggestion song)
    {
        storedSongs.Remove(song);
        RefreshStoredSongsUI();
    }

    // Added for Up/Down reordering
    public void MoveSongUp(SongSuggestion song)
    {
        int index = storedSongs.IndexOf(song);
        if (index > 0)
        {
            storedSongs.RemoveAt(index);
            storedSongs.Insert(index - 1, song);

            RefreshStoredSongsUI();
        }
    }

    public void MoveSongDown(SongSuggestion song)
    {
        int index = storedSongs.IndexOf(song);
        if (index < storedSongs.Count - 1)
        {
            storedSongs.RemoveAt(index);
            storedSongs.Insert(index + 1, song);

            RefreshStoredSongsUI();
        }
    }


    // [System.Serializable]
    // public class SongSuggestion
    // {
    //     public string title;
    //     public string artist;
    //     public string video_id;
    // }


    public void AddDownloadedSongPath(string path, string genre) //added genre to the parameters to take both path and genre of song
    {
    //     if (!downloadedSongPaths.Contains(path))
    // {
    //     downloadedSongPaths.Add(path);
       //if (!string.IsNullOrEmpty(path) && !downloadedSongPaths.Contains(path))
        //{
        //    downloadedSongPaths.Add(path);
        //}
        if (!string.IsNullOrEmpty(path))
        {

            //if the genre get return as empty return genre as unknown.
            if (string.IsNullOrEmpty(genre))
            {
                genre = "unknown";
            }

            //downloadedSongPaths.Add(path);
            downloadedSongs.Add(new DownloadedSong(path, genre)); //updated this line to include genre
            Debug.Log("[SongManager] Added downloaded path: " + path + " (total now: " + downloadedSongs.Count + ")");
            Debug.Log("Genre is: " + genre); //added debug to see the genre in terminal
        }
    }

    public void ResetAllSongData()
    {
        Debug.Log("[SongManager] ResetAllSongData() called — clearing playlist and downloads");

        Debug.Log("[SongManager] storedSongs BEFORE clear: " + storedSongs.Count);
        storedSongs.Clear();
        Debug.Log("[SongManager] storedSongs AFTER clear: " + storedSongs.Count);
        selectedSong = null;
        downloadedSongs.Clear();//Clear old songs before adding new ones so nothing gets mixed up
        downloadedById.Clear();

        if (storedSongsContainer != null)
            RefreshStoredSongsUI();
    }

    private void OnLevelWasLoaded(int level)
    {
        if (storedSongsContainer == null)
        {
            var go = GameObject.Find("StoredPenal");
            if (go != null)
                storedSongsContainer = go.transform;
        }

        RefreshStoredSongsUI();
    }

}