using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{

    public TMP_Text scoreText;

    // Update is called once per frame
    void Update()
    {
        // some safety checks for errors
        if (scoreText == null) return;
        if (GameManager.Instance == null) return;

        scoreText.text = "Score: " + GameManager.Instance.currentScore; // grabbing score data 
    }
}
