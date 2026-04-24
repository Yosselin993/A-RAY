using UnityEngine;

public class MusicSetupUI : MonoBehaviour
{
    public Transform storedSongsContainer;
    public GameObject storedSongPrefab;

    void Start()
    {
        // Reconnect UI to the persistent SongManager
        SongManager.Instance.storedSongsContainer = storedSongsContainer;
        SongManager.Instance.storedSongPrefab = storedSongPrefab;

        // Refresh UI in case SongManager already has songs
        SongManager.Instance.RefreshStoredSongsUI();
    }
}