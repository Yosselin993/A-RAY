using UnityEngine; //required for all unity scripts :)
using UnityEngine.UI;

public class BackgroundScroll : MonoBehaviour //this will be the behavior

{
    //Determines the speed of scrolling (Change higher if faster speed needed)
    public float scrollSpeed = 0.3f;
    // Background needs to be RAWIMAGE
    private RawImage rawImage;

    private float offset; //According to unity's references, keep tracks of amount (in this case scrolling)

    void Start()
    {
        rawImage = GetComponent<RawImage>(); //Will get rawimage to this object
    }

    void Update()
    {
        // The effect of .scrolling
        // As shown in to visual studio, the more it scrolls, it increases offset overtime
        offset += scrollSpeed * Time.deltaTime;

        // Will wrap the offset (make scene repeat) back to the beginning when it reaches the end
        offset = Mathf.Repeat(offset, 1f);

        rawImage.uvRect = new Rect(offset, 0f, 1f, 1f);
    }
}
