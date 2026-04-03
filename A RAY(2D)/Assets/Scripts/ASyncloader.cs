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
        MusicPlayer.SetActive(false);
        loadingScreen.SetActive(true);

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
        loadingSlider.value = progressValue;
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

        SongManager.Instance.downloadedSongPaths.Clear();
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
                        SongManager.Instance.AddDownloadedSongPath(response.path);
                        Debug.Log("Stored downloaded path: " + response.path);
                        Debug.Log("Total downloaded songs in SongManager: " + SongManager.Instance.downloadedSongPaths.Count);
                    }
                    else
                    {
                        Debug.LogError("Download succeeded but no path was returned.");
                    }
                }
            }

            // // update slider
            // if (loadingSlider != null)
            // {
            //     loadingSlider.value = (float)(i + 1) / totalSongs;
            // }

            // REBUILD downloadedById now that downloadedSongPaths is populated
            var stored = SongManager.Instance.GetStoredSongs();
            for (int j = 0; j < stored.Count; j++)
            {
                var s = stored[j];
                if (s != null && j < SongManager.Instance.downloadedSongPaths.Count)
                    SongManager.Instance.downloadedById[s.video_id] = SongManager.Instance.downloadedSongPaths[j];
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
        public string message;
    }



}