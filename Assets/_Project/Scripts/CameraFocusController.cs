
using System;
using UnityEngine;
using Vuforia;

public class CameraFocusController : MonoBehaviour
{
    void Awake()
    {
        VuforiaApplication.Instance.OnVuforiaInitialized += VuforiaInitialized;
        VuforiaApplication.Instance.OnVuforiaStarted += VuforiaStarted;
        VuforiaApplication.Instance.OnVuforiaPaused += Paused;
    }

    private void VuforiaInitialized(VuforiaInitError error)
    {
        try
        {
            if (error != VuforiaInitError.NONE)
            {
                Debug.LogError($"[VuforiaInitialized] Error: {error}");
            }
            else
            {
                Debug.Log($"[VuforiaInitialized] FocusMode: ContinuousAuto");

                VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(
                    FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

                //VuforiaBehaviour.Instance.CameraDevice.SetCameraMode(Vuforia.CameraMode.MODE_DEFAULT);
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"[VuforiaInitialized] Exception: {e.Message}");
        }
    }

    private void VuforiaStarted()
    {
        try
        {
            Debug.Log($"[VuforiaStarted]");
        }
        catch (Exception e)
        {
            Debug.LogError($"[VuforiaStarted] Exception: {e.Message}");
        }
    }
    
    public void Paused(bool paused)
    {
        try
        {
            if (!paused) // resumed
            {
                EnableCameraAndTracking(true);
                
                // Set again autofocus mode when app is resumed
                VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(
                   FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

                Debug.Log($"[Unpaused] FocusMode: FOCUS_MODE_INFINITY");
            }
            else
            {
                VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(
                   FocusMode.FOCUS_MODE_INFINITY);
                
                EnableCameraAndTracking(false);

                Debug.Log($"[Paused] FocusMode: Normal");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[VuforiaInitialized] Exception: {e.Message}");
        }
    }
    
    public void EnableCameraAndTracking(bool enableDeviceTracking)
    {
        VuforiaBehaviour.Instance.DevicePoseBehaviour.enabled = enableDeviceTracking;
        VuforiaBehaviour.Instance.enabled = enableDeviceTracking;
    }
}