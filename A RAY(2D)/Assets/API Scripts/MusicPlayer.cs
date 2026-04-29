using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private int currentIndex = 0;
    private string lastPlayedPath = null;

    //Local list of paths to play (so we can keep it functioning here)
   // private List<string> localPlaylist = new List<string>();

   //this is the playlist that will be used in the game
   // this will now store both genre ans file path (again due to the new class)
    private List<DownloadedSong> localPlaylist = new List<DownloadedSong>();
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

       // NEW: Build our own internal playlist using the already-working data
        BuildLocalPlaylist();

        PlayCurrentSong();
    }

    void BuildLocalPlaylist()
    {
        localPlaylist.Clear();

        var playlist = SongManager.Instance.GetStoredSongs();
        //var paths = SongManager.Instance.downloadedSongPaths;
        var paths = SongManager.Instance.downloadedSongs; //updated to include both file path and genre

        // Build playlist 1:1 with storedSongs
        for (int i = 0; i < playlist.Count; i++)
        {
            if (i < paths.Count)
            {
                //gets the current song from list (with both path and genre)
                var song = paths[i];
                //string p = paths[i];
                string p = song.path; //get the file path from 'song' 
                string genre = song.genre; //gets the genre from 'song'

                // Normalize path
                if (!Path.IsPathRooted(p))
                {
                    string projectRoot = Directory.GetParent(Application.dataPath).FullName;
                    p = Path.Combine(projectRoot, p);
                }

                p = Path.GetFullPath(p);

                Debug.Log("[DEBUG] Added normalized path to playlist: " + p);
                Debug.Log("Genre is: " + genre);
                //localPlaylist.Add(p);
                //this updated version add the song with all data (path and genre)
                localPlaylist.Add(new DownloadedSong(p, genre));
            }
            else
            {
                Debug.LogWarning("No path for song index: " + i);
            }
        }

        Debug.Log("LOCAL PLAYLIST BUILT. Count = " + localPlaylist.Count);
        Debug.Log("=== STORED SONGS ORDER ===");
        for (int i = 0; i < playlist.Count; i++)
        {
            Debug.Log(i + ": " + playlist[i].title + " (" + playlist[i].video_id + ")");
        }
        currentIndex = 0;
    }

    void PlayCurrentSong()
    {
        Debug.Log("PlayCurrentSong() called. Index = " + currentIndex + " / Count = " + localPlaylist.Count);

        if (localPlaylist.Count == 0)
            return;

        if (currentIndex < 0 || currentIndex >= localPlaylist.Count)
            return;

        //string path = localPlaylist[currentIndex];
        var song = localPlaylist[currentIndex];
        string path = song.path;
        string genre = song.genre;
        Debug.Log("PLAYING SONG WITH GENRE: " + genre);

        Debug.Log("[DEBUG] Raw playlist path: " + path);

        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning("Path is null or empty, skipping...");
            PlayNextSong();
            return;
        }

        // FIX: Compare paths, not clip names
        Debug.Log("[DEBUG] Duplicate check: lastPlayedPath=" + lastPlayedPath + " | current=" + path);

        if (lastPlayedPath == path)
        {
            Debug.Log("[DEBUG] Duplicate detected — resetting AudioSource");
            audioSource.Stop();
            audioSource.clip = null;
        }

        StartCoroutine(LoadAndPlay(path));
    }

    IEnumerator LoadAndPlay(string path)
    {
        // Normalize path
        if (!Path.IsPathRooted(path))
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            path = Path.Combine(projectRoot, path);
        }
        Debug.Log("Attempting to load path: " + path);

        if (!File.Exists(path))
        {
            Debug.LogError("File not found: " + path);
            yield break;
        }

        string uri = new System.Uri(path).AbsoluteUri;
        Debug.Log("[DEBUG] URI used for loading: " + uri);

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

                Debug.Log("[DEBUG] Clip loaded successfully. Length=" + clip.length);

                audioSource.Stop();
                audioSource.clip = clip;
                audioSource.Play();

                Debug.Log("[DEBUG] Now playing clip. isPlaying=" + audioSource.isPlaying);

                // Update last played path
                lastPlayedPath = path;
                Debug.Log("[DEBUG] Updated lastPlayedPath=" + lastPlayedPath);

                StartCoroutine(WaitForSongToEnd(Mathf.Min(60f, clip.length)));
            }
            else
            {
                Debug.LogError("There is something wrong with www: " + www.error);
            }
        }
    }

    IEnumerator WaitForSongToEnd(float duration)
    {
        Debug.Log("[DEBUG] Waiting for song to finish. Duration=" + duration);

        // Wait for the duration of the clip
        yield return new WaitForSeconds(duration);

        // Small buffer to ensure playback is truly finished
        yield return new WaitForSeconds(0.1f);

        Debug.Log("[DEBUG] Song finished — calling PlayNextSong()");
        PlayNextSong();
    }

    void PlayNextSong()
    {
        Debug.Log("[DEBUG] Advancing to next song. Old index=" + currentIndex);

        currentIndex++;

        // NEW: Use our local playlist count
        if (currentIndex >= localPlaylist.Count)
        {
            Debug.Log("End of playlist.");
            return;
        }

        Debug.Log("[DEBUG] New index=" + currentIndex);
        PlayCurrentSong();
    }
}