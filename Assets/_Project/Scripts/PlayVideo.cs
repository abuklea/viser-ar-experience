
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    public string videoURL;
    public GameObject progressIndicator;
    public GameObject videoSurface;
    public VideoPlayer videoPlayer;
    public Material surfaceMaterial;
    public MeshRenderer[] surfaceMeshRenderers;

    bool playing = false;
    Coroutine co;
    bool initialised = false;
    MeshFilter targetMesh;
    
    [Button]
    public void UpdateSubComponents()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>(true);
        videoPlayer.url = videoURL;
        videoPlayer.targetMaterialRenderer = videoSurface.GetComponent<MeshRenderer>();

        if (surfaceMeshRenderers != null && surfaceMaterial != null)
        {
            foreach (var ren in surfaceMeshRenderers)
            {
                ren.sharedMaterial = surfaceMaterial;
            }
        }
    }

    private void Awake()
    {
        UpdateSubComponents();
        
        if (!initialised)
        {
            videoPlayer.gameObject.SetActive(false);
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = videoURL;
            videoPlayer.playOnAwake = false;
            videoPlayer.waitForFirstFrame = true;
            videoPlayer.skipOnDrop = true;
            initialised = true;
            videoPlayer.gameObject.SetActive(true);
        }
    }

    void OnEnable()
    {
        if (co != null)
            StopCoroutine(co);
        
        co = StartCoroutine(DoLoadVideo());
    }

    void OnDisable()
    {
        if (co != null)
            StopCoroutine(co);

        co = null;
        playing = false;
        videoPlayer.Stop();
        videoPlayer.frame = 0;

        videoSurface.SetActive(false);
    }

    IEnumerator DoLoadVideo()
    {
        //if (progressIndicator != null)
        progressIndicator.SetActive(true);

        playing = true;

        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            if (!playing)
                yield break;

            yield return null;
        }

        //if (progressIndicator != null)
        progressIndicator.SetActive(false);

        videoSurface.SetActive(true);
        yield return null;
        videoPlayer.Play();
    }
}
