
using UltEvents;
using UnityEngine;

public class OnKey : MonoBehaviour
{
    public KeyCode key = KeyCode.Escape;
    public UltEvent Pressed;
    
    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Pressed?.Invoke();
        }
    }
}
