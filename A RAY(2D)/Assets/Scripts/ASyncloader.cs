using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASnycloader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject MusicPlayer;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;
    public void LoadLevelBtn(string levelToLoad)
    {
        MusicPlayer.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelASync(levelToLoad));
    }
    IEnumerator LoadLevelASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
        float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
        loadingSlider.value = progressValue;
        yield return null;
        }
    }
}