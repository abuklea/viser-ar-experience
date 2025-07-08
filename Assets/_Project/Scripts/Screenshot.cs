
using UnityEngine;

[ExecuteInEditMode]
public class Screenshot : MonoBehaviour
{
    static int index = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Screenshot!");

            ScreenCapture.CaptureScreenshot($"Image_{index++}.jpg");
        }
    }
}