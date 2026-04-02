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

    // Added for Up/Down buttons would be easier to do so rather than a single button
    public void MoveUp()
    {
        SongManager.Instance.MoveSongUp(song);
    }

    public void MoveDown()
    {
        SongManager.Instance.MoveSongDown(song);
    }
}
