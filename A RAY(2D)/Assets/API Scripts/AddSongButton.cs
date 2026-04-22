using UnityEngine;
using UnityEngine.UI;

public class AddSongButton : MonoBehaviour
{
    private void Awake()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            SongManager.Instance.AddSelectedSong();
        });
    }
}