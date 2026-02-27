using UnityEngine;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;

    public Difficulty currentDifficulty = Difficulty.Medium;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
