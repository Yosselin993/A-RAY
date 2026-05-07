using UnityEngine;
using TMPro;

public class PredictionDisplay : MonoBehaviour
{
    public LogRegModel model;          // Drag your LogRegModel here
    public DataCollector data;         // Drag your DataCollector here
    public TextMeshProUGUI textUI;     // Drag your TMP text here

    void Start()
    {
        // Update every sample interval (15 seconds)
        InvokeRepeating(nameof(UpdatePrediction), 0f, data.sampleInterval);
    }

    void UpdatePrediction()
    {
        float p = model.Predict(data);
        float percent = p * 100f;

        textUI.text = "Chance of Top 5: " + percent.ToString("F1") + "%";
    }
}
