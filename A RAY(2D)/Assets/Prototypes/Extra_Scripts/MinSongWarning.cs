
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; // needed for cliking detection


public class MinSongWarning : MonoBehaviour, IPointerClickHandler
{
    public CanvasGroup cdCanvasGroup; // i Want our CD to fade in to give it an effect
    public RectTransform boxTransform; //expanding box
    public TMP_Text warningText;

    // FOCUSING ON THE ANIMATIONS HERE
    public float fadeDuration = 0.8f;
    public float expandSpeed = 500f;
    public float targetWidth = 400f; //width

    private float currentWidth = 0f;
    private Coroutine currentRoutine;

    private Coroutine fadeRoutine;
    private void Start()
    {
        cdCanvasGroup.alpha = 0f;

        Vector2 size = boxTransform.sizeDelta;
        currentWidth = 0f;
        boxTransform.sizeDelta = new Vector2(0f, size.y);
        cdCanvasGroup.blocksRaycasts = false;
        cdCanvasGroup.interactable = false;
        
       
    }

    public void ShowIfNeeded()
    {
        
        if (SongManager.Instance == null) return;
        if (SongManager.Instance.GetStoredSongs().Count < SongManager.Instance.minsonglist)
        {
            warningText.text = "Minimum of 5 songs required";


            if (currentRoutine != null)
                StopCoroutine(currentRoutine);

            currentRoutine = StartCoroutine(Playit());
            
        }
    }


    IEnumerator Playit()
    {
        cdCanvasGroup.blocksRaycasts = true;
        cdCanvasGroup.interactable = true;
        // resets here
        cdCanvasGroup.alpha = 0f;
        currentWidth = 0f;
        Vector2 boxSize = boxTransform.sizeDelta;
        boxTransform.sizeDelta = new Vector2(0f, boxSize.y);

        float t = 0f;
        // CD and box size
        while (t < 1f ||  currentWidth < targetWidth)
        {
            if (t <1f)
            {
                t += Time.deltaTime / fadeDuration;
                cdCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            }
            //box
            currentWidth = Mathf.MoveTowards(
                currentWidth,
                    targetWidth,
                    Time.deltaTime* expandSpeed
            );
            boxTransform.sizeDelta = new Vector2(currentWidth, boxSize.y);
            yield return null;
        }
        cdCanvasGroup.alpha = 1f;
        boxTransform.sizeDelta = new Vector2(targetWidth, boxSize.y);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Hide();
    }
    public void Hide()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeOut());
    }


    IEnumerator FadeOut()
    {
        float startAlpha = cdCanvasGroup.alpha;
        float t = 0f;
        Vector2 boxSize = boxTransform.sizeDelta;

        while (t< fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
            cdCanvasGroup.alpha = alpha;

            yield return null;
        }
        cdCanvasGroup.alpha = 0f; //basically putting everything invisible again
        boxTransform.sizeDelta = new Vector2(0f,boxSize.y);
        currentWidth = 0f;
        cdCanvasGroup.blocksRaycasts = false;
        cdCanvasGroup.interactable = false;
    }

}
