using UnityEngine;

public class StoredSongPrefabScript : MonoBehaviour
{
   private SongSuggestion song;

    public void Setup(SongSuggestion s)
    {
        song = s;
    }

    public void DeleteThisSong()
    {
        SongManager.Instance.DeleteSong(song);
    }
}
