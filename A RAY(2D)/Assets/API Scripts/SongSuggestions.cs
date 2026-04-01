using UnityEngine;

public class SongSuggestions : MonoBehaviour
{
   [System.Serializable]
    public class SongSuggestion
    {
        public string title;
        public string artist;
        public string video_id;
    }
}
