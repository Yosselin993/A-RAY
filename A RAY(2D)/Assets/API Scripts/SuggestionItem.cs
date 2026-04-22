using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuggestionItem : MonoBehaviour
{
    public TextMeshProUGUI label;

    private string videoId;
    private string title;
    private string artist;

    private void Awake()
    {
        // Ensure the button always has the OnClick listener
        Button btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnClick);
    }
    
    public void Setup(string title, string videoId, string artist)
    {
        this.title = title;
        this.videoId = videoId;
        this.artist = artist;

        if (label != null)
        {
            label.text = title + " - " + artist;
        }
    }

    public void OnClick()
    {
        SongSuggestion song = new SongSuggestion();
        song.title = title;
        song.video_id = videoId;
        song.artist = artist;

        SongManager.Instance.SetSelectedSong(song);
        SongManager.Instance.AddSelectedSong();
    }

}