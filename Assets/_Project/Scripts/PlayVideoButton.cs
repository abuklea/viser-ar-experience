
using RyanQuagliataUnity;
using System;
using UltEvents;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Button))]
public class PlayVideoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Serializable]
    public enum DeviceOS
    {
        iOS = 0,
        Android = 1
    }
    
    [Serializable]
    public class VideoDictionary : SerializedDictionary<DeviceOS, string> { }

    public PlayFullscreenVideo fullscreenVideoPlayer;
    
    [SerializeReference]
    public VideoDictionary fullscreenVideos = new VideoDictionary();
    
    public ScreenOrientation screenOrientation = ScreenOrientation.LandscapeLeft;
    public bool playOnRelease = true;
    
    public UltEvent Pressed;
    public UltEvent Released;

    [Button]
    public void Setup()
    {
        //fullscreenVideos = new VideoDictionary();
        //string android = Released.PersistentCallsList[0].PersistentArguments[0].String;
        //string ios = android.Replace(".webm",".mkv");

        //fullscreenVideos.Add(DeviceOS.Android, android);

        if (fullscreenVideos.ContainsKey(DeviceOS.iOS))
        {
            string ios = fullscreenVideos[DeviceOS.iOS];
            ios = ios.Replace(".mkv", ".mp4");
            fullscreenVideos[DeviceOS.iOS] = ios;
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (playOnRelease && fullscreenVideos != null && fullscreenVideoPlayer != null)
        {
#if UNITY_IOS            
            if (fullscreenVideos.ContainsKey(DeviceOS.iOS) && 
                !string.IsNullOrEmpty(fullscreenVideos[DeviceOS.iOS]))
            {
                fullscreenVideoPlayer.PlayFullscreen(fullscreenVideos[DeviceOS.iOS], screenOrientation);
            }
#elif UNITY_ANDROID
            if (fullscreenVideos.ContainsKey(DeviceOS.Android) &&
                     !string.IsNullOrEmpty(fullscreenVideos[DeviceOS.Android]))
            {
                fullscreenVideoPlayer.PlayFullscreen(fullscreenVideos[DeviceOS.Android], screenOrientation);
            }            
#endif
        }
        
        Released?.Invoke();
    }
}