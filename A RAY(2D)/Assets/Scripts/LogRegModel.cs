using UnityEngine;

public class LogRegModel : MonoBehaviour
{
    // Logistic regression weights (W) and bias (b)
    private readonly float[] W = new float[8] {
        2.82442544f, 1.5697378f, 0.56824782f, 1.21991608f,
        -0.16872207f, -0.08997148f, -0.68901406f, -0.02677207f
    };

    private readonly float b = 0.00901878f;

    // Scaler mean and scale
    private readonly float[] mean = new float[8] {
        659.259259f, 3.89382716f, 0.220740741f, 2.8691358f,
        8.51028745f, -0.0342386807f, 1479.25926f, 100.684594f
    };

    private readonly float[] scale = new float[8] {
        479.191134f, 1.73449716f, 0.258608708f, 1.30721096f,
        8.75846989f, 0.0615461736f, 135.771938f, 79.3076188f
    };

    float Normalize(float value, int i)
    {
        return (value - mean[i]) / scale[i];
    }

    float Sigmoid(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }

   public float Predict(DataCollector data)
    {
        float[] x = new float[8];
        x[0] = Normalize(GameManager.Instance.currentScore, 0);
        x[1] = Normalize(FindFirstObjectByType<PlayerHealth>().currentHealth, 1);
        x[2] = Normalize(data.GetPlaylistProgress(), 2);
        x[3] = Normalize(data.GetSongsRemaining(), 3);
        x[4] = Normalize(data.GetAverageScoreRate(), 4);
        x[5] = Normalize(data.GetHealthDecayRate(), 5);
        x[6] = Normalize(data.GetLeaderboardThreshold(), 6);
        x[7] = Normalize(data.GetTimeElapsed(), 7);

        float z = b;
        for (int i = 0; i < 8; i++)
            z += W[i] * x[i];

        return Sigmoid(z);
    }
}