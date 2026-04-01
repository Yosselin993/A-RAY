using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ExitGameHandler : MonoBehaviour
{
    public void ClearDownloads()
    {
        StartCoroutine(ClearDownloadsRoutine());
    }

    private IEnumerator ClearDownloadsRoutine()
    {
        string url = "http://127.0.0.1:8000/clear_downloads";

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, ""))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Clear error: " + request.error);
                yield break;
            }

            Debug.Log("Downloads cleared.");
            SongManager.Instance.downloadedSongPaths.Clear();
        }
    }
}