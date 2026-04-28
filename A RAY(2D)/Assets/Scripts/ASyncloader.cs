using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class ASnycloader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject MusicPlayer;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;
    public void LoadLevelBtn(string levelToLoad)
    {
        //Require at least 5 songs before entering the game
    if (SongManager.Instance.GetStoredSongs().Count < SongManager.Instance.minsonglist)
    {
        Debug.Log("You must select at least 5 songs before entering the game.");
        return; // Block loading
    }

        MusicPlayer.SetActive(false);
        loadingScreen.SetActive(true);

        if(loadingSlider != null)
            loadingSlider.value = 0f; //manually set the the progress to 0 if theres nothing to load



        StartCoroutine(BeginLoad(levelToLoad));
    }

        IEnumerator BeginLoad(string levelToLoad)
    {
        yield return StartCoroutine(DownloadStoredSongs());
        yield return StartCoroutine(LoadLevelASync(levelToLoad));
    }


    IEnumerator LoadLevelASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
        float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);

        if (loadingSlider != null)
            {
                //manually set the the progress of the game scene stuff to load to take up 20% of the silder
                 loadingSlider.value = 0.8f + (progressValue * 0.2f);
            }
        yield return null;
        }
    }

    IEnumerator DownloadStoredSongs()
    {
        if (SongManager.Instance == null)
        {
            Debug.LogError("SongManager not found!");
            yield break;
        }

        //SongManager.Instance.downloadedSongPaths.Clear();
        // Clear all previously stored downloaded songs (for both path and genre because of the new class downloadedSongs)
        SongManager.Instance.downloadedSongs.Clear();
        // Clear the dictionary that maps videoId to file path
        SongManager.Instance.downloadedById.Clear();
        // Clear the dictionary that maps videoId to genre
        SongManager.Instance.genreById.Clear();



        var songs = SongManager.Instance.GetStoredSongs();

        if (songs == null || songs.Count == 0)
        {
            Debug.Log("No songs to download.");
            yield break;
        }

        int totalSongs = Mathf.Min(songs.Count, 8);

        for (int i = 0; i < totalSongs; i++)
        {
            var song = songs[i];

           string url =
                "http://127.0.0.1:8000/download_song?video_id=" +
                UnityWebRequest.EscapeURL(song.video_id) +
                "&title=" +
                UnityWebRequest.EscapeURL(song.title) +
                "&artist=" +
                UnityWebRequest.EscapeURL(song.artist);

            Debug.Log("Downloading: " + song.title);

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

            
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Download failed: " + request.error);
                }
                else
                {
                    Debug.Log("Download complete: " + request.downloadHandler.text);
                    DownloadSongResponse response = JsonUtility.FromJson<DownloadSongResponse>(request.downloadHandler.text);
                    if (response != null && !string.IsNullOrEmpty(response.path))
                    {
                        SongManager.Instance.AddDownloadedSongPath(response.path, response.genre); //adding the genre to the preameter
                        Debug.Log("Stored downloaded path: " + response.path);
                        Debug.Log("Stored genre: " + response.genre); //added debugto see if genre got stored
                        Debug.Log("Total downloaded songs in SongManager: " + SongManager.Instance.downloadedSongs.Count);
                    }
                    else
                    {
                        Debug.LogError("Download succeeded but no path was returned.");
                    }
                }
            }

            // // update slider
            //if the loadingSliler isnot equal to null
            if (loadingSlider != null)
            {
                //I created a new variable that will track the progress of the downloading songs
                float downloadprogress = (float)(i + 1) / totalSongs;
                loadingSlider.value = downloadprogress * 0.8f; //manually set the downloadprogress to take up 80% of the slider

            }

            // REBUILD downloadedById now that downloadedSongPaths is populated
            var stored = SongManager.Instance.GetStoredSongs();
            for (int j = 0; j < stored.Count; j++)
            {
                var s = stored[j];
                if (s != null && j < SongManager.Instance.downloadedSongs.Count) //change to add new class
                    //SongManager.Instance.downloadedById[s.video_id] = SongManager.Instance.downloadedSongPaths[j];//come back and fix.
                {   // Store the downloaded file path using the song's video ID
                    SongManager.Instance.downloadedById[s.video_id] = SongManager.Instance.downloadedSongs[j].path;

                    // Store the genre using the same video ID so we can access it later (AKA quicker)
                    SongManager.Instance.genreById[s.video_id] = SongManager.Instance.downloadedSongs[j].genre;
                }
            }

            Debug.Log("Rebuilt downloadedById. Count = " + SongManager.Instance.downloadedById.Count);
        }
    }

    //  IEnumerator LoadLevelAsync(string levelToLoad)
    // {
    //     AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

    //     while (!loadOperation.isDone)
    //     {
    //         yield return null;
    //     }
    // }

    [System.Serializable]
    public class DownloadSongResponse
    {
        public string status;
        public string path;
        public string genre; //adding the genre to the class
        public string message;
    }



}