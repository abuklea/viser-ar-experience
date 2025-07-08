
using System;
using System.Collections;
using UnityEngine;

public class PlayFullscreenVideo : MonoBehaviour
{
    public GameObject blackoutOverlay;
    public GameObject blackoutIcon;
    public ScreenOrientation videoOrientation = ScreenOrientation.AutoRotation;
    
    private WaitForSeconds wfs;

    private void Awake()
    {
        wfs = new WaitForSeconds(0.4f);
    }

    public void PlayFullscreen(string url, ScreenOrientation orientation = ScreenOrientation.AutoRotation)
    {
        if (string.IsNullOrEmpty(url))
            return;
        
        Uri uriResult;
        if (Uri.TryCreate(url, UriKind.Absolute, out uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
        {
            StartCoroutine(DoPlayFullscreen(url, orientation));
        }
    }
    
    IEnumerator DoPlayFullscreen(string url, ScreenOrientation orientation = ScreenOrientation.AutoRotation)
    {
        if (blackoutOverlay)
            blackoutOverlay.SetActive(true);
        
        yield return wfs;
        
        if(blackoutIcon)
            blackoutIcon.SetActive(false);
        
        //Screen.orientation = ScreenOrientation.LandscapeLeft;
        //yield return wfs;
        
        Handheld.PlayFullScreenMovie(url, Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.AspectFit);
        yield return wfs;
        
        //Screen.orientation = ScreenOrientation.Portrait;
        //yield return wfs;
        
        if(blackoutIcon)
            blackoutIcon.SetActive(true);
        
        yield return wfs;
        
        if (blackoutOverlay)
            blackoutOverlay.SetActive(false);
    }
}