using UnityEngine;
using UnityEngine.UI; // this is needed as I will be including something for the UI

public class ColorChange : MonoBehaviour
{
    private Image targetImage;
    // Now for the colors themselves (I wanted two....)
    // will include SerializeField to display these private Color1 and Color2 into the inspector tab for us to be able to change color
    [SerializeField] private Color color1 = Color.white;
    [SerializeField] private Color color2 = Color.black;

    private float fadeDuration = 2f;

    // Happens when game is running...
    private void Awake()
    {
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>(); //so if nothing shows i'll just get the default
        }
    }
    private void Update()
    {
        //Updating into the game
        // Looking through Unity's document sheet, the best course here is to use Mathf.PingPong as it returns 0 to the length  (basically back and forth)
        float t = Mathf.PingPong(Time.time / fadeDuration, 1f);
        //Then it blends with lerp
        targetImage.color = Color.Lerp(color1, color2, t);
    }


}
