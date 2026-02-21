using UnityEngine;

public class Spinning : MonoBehaviour
{
    // This is the rotation speed - 
    public float rotationSpeed = 100f;

    void Update()
    {
        // I want it for the disk sprite so 2D.
        // This means rotating through Z
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
