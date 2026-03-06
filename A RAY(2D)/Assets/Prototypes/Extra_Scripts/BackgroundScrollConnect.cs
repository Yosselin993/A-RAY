using UnityEngine; // Required for all Unity based Code
using UnityEngine.UI;

// this is for the multiple scrolling backgrounds
public class BackgroundScrollConnect : MonoBehaviour //this will be the behavior

{
    //Determines the speed of scrolling (Change higher if faster speed needed)
    public float scrollSpeed = 0.3f;
    // Background needs to be RAWIMAGE
    public Texture[] backgrounds; //Needed for my list of backgrounds

    private RawImage rawImage;

    private float offset; //According to unity's references, keep tracks of amount (in this case scrolling)
    private int lastIndex = -1;

    void Start()
    {
        rawImage = GetComponent<RawImage>(); //Will get rawimage to this object

        ChangeBackground();
    }

    void Update()
    {
      
        // The effect of .scrolling
        // As shown in to visual studio, the more it scrolls, it increases offset overtime
        offset += scrollSpeed * Time.deltaTime;

        offset = Mathf.Repeat(offset, 1f);
       
        rawImage.uvRect = new Rect(offset, 0f, 1f, 1f);


    }
    // making this function public...
     public void ChangeBackground()
    {
        if (backgrounds.Length == 0) return;
        int newIndex;
        do
        {
            newIndex = Random.Range(0, backgrounds.Length);
        }
        while (newIndex == lastIndex && backgrounds.Length > 1);

        lastIndex = newIndex;
        rawImage.texture = backgrounds[newIndex];
    }
}
