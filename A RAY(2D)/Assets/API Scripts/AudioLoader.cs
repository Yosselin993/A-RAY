using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AudioLoader : MonoBehaviour
{
    public IEnumerator LoadAllSongs(System.Action<List<AudioClip>> onComplete)
    {
        List<AudioClip> clips = new List<AudioClip>();

        foreach (string path in SongManager.Instance.downloadedSongPaths)
        {
            string fullPath = "file:///" + System.IO.Path.GetFullPath(path);

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fullPath, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error loading audio: " + www.error);
                    continue;
                }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                clips.Add(clip);
            }
        }

        onComplete?.Invoke(clips);
    }
}