
using MagneticScrollView;
using UnityEngine;

public class ContentScrollerSetup : MonoBehaviour
{
    void Start()
    {
        MagneticScrollRect scroller = GetComponent<MagneticScrollRect>();
        if (scroller != null)
            scroller.ArrangeElements();
    }
}
