using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TextBoxScript : MonoBehaviour
{
    public TMP_InputField inputField;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(SearchSong(inputField.text));
        }
    }

    IEnumerator SearchSong(string query)
    {
        string url = "http://127.0.0.1:8000/search?q=" + UnityWebRequest.EscapeURL(query);

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            yield break;
        }

        UnityEngine.Debug.Log("Search Results: " + request.downloadHandler.text);
    }
}