using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class AutocompleteController : MonoBehaviour
{
    public TextBoxScript textBoxScript; // reference to search script
    public TMP_InputField searchInput;
    public GameObject suggestionPrefab;
    public Transform suggestionContainer;

    private float typingDelay = 0.25f;
    private Coroutine typingCoroutine;

    void Start()
    {
        // Add a listener to the input field so that every time the text changes,
        // the OnTextChanged function is automatically called
        searchInput.onValueChanged.AddListener(OnTextChanged);
        suggestionContainer.gameObject.SetActive(false); //hiding dropdown panel
    }

    void OnTextChanged(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // If the input field is empty or only contains spaces
         if (string.IsNullOrWhiteSpace(text))
        {
            // Clear any existing suggestions from the UI
            ClearSuggestions();
            // Hide the suggestion dropdown panel
            suggestionContainer.gameObject.SetActive(false); // hide when the textbox is empty
            // Stop further execution
            return;
        }
        // If the user is typing something valid, show the dropdown panel
        suggestionContainer.gameObject.SetActive(true); // show when user is typing song

        // Start a delayed search coroutine (adds a small delay before searching) (will change this part)
        typingCoroutine = StartCoroutine(DelayedSearch(text));
    }

    IEnumerator DelayedSearch(string text)
    {
        yield return new WaitForSeconds(typingDelay);

        if (string.IsNullOrWhiteSpace(text))
        {
            ClearSuggestions();
            yield break;
        }

        StartCoroutine(FetchSuggestions(text));
    }

    IEnumerator FetchSuggestions(string query)
    {
        string url = "http://127.0.0.1:8000/autocomplete?q=" + UnityWebRequest.EscapeURL(query);

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            yield break;

        string json = request.downloadHandler.text;
        SuggestionList list = JsonUtility.FromJson<SuggestionList>("{\"items\":" + json + "}");

        DisplaySuggestions(list.items);
    }


    void DisplaySuggestions(List<SongSuggestion> suggestions)
    {
        ClearSuggestions();

        int count = Mathf.Min(3, suggestions.Count);

        for (int i = 0; i < count; i++)
        {
            var s = suggestions[i];

            GameObject obj = Instantiate(suggestionPrefab, suggestionContainer);
            obj.GetComponentInChildren<TMP_Text>().text = $"{s.title} - {s.artist}";

            obj.GetComponent<Button>().onClick.AddListener(() =>
            {
                searchInput.text = s.title + " - " + s.artist;

                SongManager.Instance.SetSelectedSong(s);

                ClearSuggestions();
            });
        }
    }



    // void DisplaySuggestions(List<SongSuggestion> suggestions)
    // {
    //     ClearSuggestions();


    //     foreach (var s in suggestions)
    //     {
    //         GameObject obj = Instantiate(suggestionPrefab, suggestionContainer);
    //         obj.GetComponentInChildren<TMP_Text>().text = $"{s.title} - {s.artist}";

    //         // obj.GetComponent<Button>().onClick.AddListener(() =>
    //         // {
    //         //     searchInput.text = s.title;
    //         //     ClearSuggestions();
    //         // });
    //        obj.GetComponent<Button>().onClick.AddListener(() =>
    //         {
    //             searchInput.text = s.title + " - " + s.artist;
    //             ClearSuggestions();
    //         });
    //     }
    // }



    void ClearSuggestions()
    {
        foreach (Transform child in suggestionContainer)
            Destroy(child.gameObject);
    }
}

[System.Serializable]
public class SongSuggestion
{
    public string title;
    public string artist;
    public string video_id;
}

[System.Serializable]
public class SuggestionList
{
    public List<SongSuggestion> items;
}