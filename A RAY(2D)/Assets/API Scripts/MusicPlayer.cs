using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        string firstSongPath = SongManager.Instance.downloadedSongPaths[0];
        StartCoroutine(LoadAndPlay(firstSongPath));
    }

    IEnumerator LoadAndPlay(string path)
    {

        if (!Path.IsPathRooted(path))
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            path = Path.Combine(projectRoot, path);
        }

        if (!File.Exists(path))
        {
            Debug.LogError("File not found: " + path);
            yield break;
        }

        string uri = new System.Uri(path).AbsoluteUri;

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);

                if (clip == null)
                {
                    Debug.LogError("AudioClip is null.");
                    yield break;
                }

                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("There is something wrong with www: " + www.error);
            }
        }
    }
}