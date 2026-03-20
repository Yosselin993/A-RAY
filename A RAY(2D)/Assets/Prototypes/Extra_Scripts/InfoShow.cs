using UnityEngine;
using System.Collections;

public class InfoShow : MonoBehaviour // This is the behaviour of the infopanel
{
    public static InfoShow instance;
    public CanvasGroup canvasGroup; // I wanted it so it fades out so...
    public float displayTime = 3f; // How long it stays visible
    public float fadeDuration = 1f;

    void Awake()
    {
        instance = this;
        canvasGroup.alpha = 0f;
    }

    void Start()
        {
        ShowPanel(); // This is going to show the panel when the scene starts
        
    }
    public void ShowPanel()
    {
        // this function will focus on when the infopanel is showing/displaying

        StopAllCoroutines();
        StartCoroutine(ShowRoutine());
    }
    IEnumerator ShowRoutine()
    {
        yield return StartCoroutine(Fade(1f));
        // What was shown in the canvasgroup inspectator shall now be true!

        // When staying visible 
        yield return new WaitForSeconds(displayTime); //actually used for duration constantly :)

        yield return StartCoroutine(Fade(0f)); // Fading out as 0f is nothing
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
    }
}
