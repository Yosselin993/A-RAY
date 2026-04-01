using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Ayncloader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject MusicSetup;
    [SerializeField] private Slider loadingSlider;

    public void LoadLevelBtn(string levelToLoad)
    {

        StartCoroutine(BeginLoad(levelToLoad));
    }

    IEnumerator BeginLoad(string levelToLoad)
    {

        loadingScreen.SetActive(true);
        MusicSetup.SetActive(false);

        yield return null;

        yield return StartCoroutine(DownloadStoredSongs());

        yield return StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    // IEnumerator LoadLevelASync(string levelToLoad)
    // {
    //     Debug.Log("Started async load");

    //     AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

    //     while (!loadOperation.isDone)
    //     {
    //         float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);

    //         if (loadingSlider != null)
    //         {
    //             loadingSlider.value = progressValue;
    //         }

    //         yield return null;
    //     }
    // }

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

            // update slider
            if (loadingSlider != null)
            {
                loadingSlider.value = (float)(i + 1) / totalSongs;
            }
        }
    }

     IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            yield return null;
        }
    }

    [System.Serializable]
    public class DownloadSongResponse
    {
        public string status;
        public string path;
        public string message;
    }
}