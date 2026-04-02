using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;

    // public TMP_Text storedSongsText;

    public Transform storedSongsContainer;
    public GameObject storedSongPrefab;



    private SongSuggestion selectedSong;

    public List<string> downloadedSongPaths = new List<string>();
    private List<SongSuggestion> storedSongs = new List<SongSuggestion>();
    // Track downloaded songs by video_id
    public Dictionary<string, string> downloadedById = new Dictionary<string, string>();

    public int maxsonglist = 8;

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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //this will be called once the player clicks on a suggested song.
    public void SetSelectedSong(SongSuggestion song)
    {
        selectedSong = song;
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
        if (storedSongs.Count >= maxsonglist)
        {
            Debug.Log("Can only select up to 8 songs");
            return;
        }

        storedSongs.Add(selectedSong);
        RefreshStoredSongsUI();
        selectedSong = null;
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



    void RefreshStoredSongsUI()
    {
        foreach (Transform child in storedSongsContainer)
        {
            Destroy(child.gameObject);
        }

      foreach (var song in storedSongs)
      {
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


    public void AddDownloadedSongPath(string path)
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
            downloadedSongPaths.Add(path);
            Debug.Log("[SongManager] Added downloaded path: " + path + " (total now: " + downloadedSongPaths.Count + ")");
        }
    }

}