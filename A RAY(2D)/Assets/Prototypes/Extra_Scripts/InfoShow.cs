using UnityEngine;
using System.Collections;
// Then a event system is necessary for click detection for the panel 
using UnityEngine.EventSystems;

public class InfoShow : MonoBehaviour, IPointerClickHandler // This is the behaviour of the infopanel
{
    public static InfoShow instance;

    public CanvasGroup canvasGroup; // I wanted it so it fades out so...
    public float fadeDuration = 0.5f;

    private bool isVisible = false;

    void Awake()
    {
        instance = this;

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        // Components in inspector window -->
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false; //basically as seen in the inspector tab

    }

    // This is for when the game starts.
    void Start()
    {
        ShowPanel(); // shall display the infopanel to beginners!

    }
    public void ShowPanel() //messes with panel here
    {
        StopAllCoroutines();

        isVisible = true;
        StartCoroutine(Fade(1f));
    }

    // Focusing on hiding the panel
    // Function to hide
    public void HidePanel()
    {
        StopAllCoroutines();
        isVisible = false;
        StartCoroutine(Fade(0f));
    }

    public void TogglePanel()
    {
        if (isVisible)
        {
            HidePanel();
        }
        else
        {
            ShowPanel();
        }

    }

    IEnumerator Fade(float target)
    {
        float start = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, target, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = target;
        bool visible = target > 0.9f;
        canvasGroup.interactable = visible; // Changing the settings in the inspector section under CanvasGroup here
        canvasGroup.blocksRaycasts = visible;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isVisible)
            HidePanel();
    }
}
