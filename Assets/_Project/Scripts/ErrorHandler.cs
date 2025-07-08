
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class ErrorHandler : MonoBehaviour
{
    public GameObject messagePanel;
    public TMP_Text txtHeader;
    public TMP_Text txtMessage;

    public VideoPlayer[] videoPlayers;

    float defaultTimeoutSecs = 5f;


    void OnEnable()
    {
        if (videoPlayers != null)
            FindObjectsOfType<VideoPlayer>(true);

        if(videoPlayers != null)
        {
            foreach(var vp in videoPlayers)
            {
                if (vp != null)
                {
                    vp.errorReceived += HandleVideoError;
                    vp.frameDropped += HandleVideoFrameDropped;
                }
            }
        }
    }

    void OnDisable()
    {
        if (videoPlayers != null)
        {
            foreach (var vp in videoPlayers)
            {
                if (vp != null)
                {
                    vp.errorReceived -= HandleVideoError;
                    vp.frameDropped -= HandleVideoFrameDropped;
                }
            }
        }
    }

    void HandleVideoError(VideoPlayer videoPlayer, string message)
    {
        //Debug.LogError($"[ErrorHandler][VideoPlayer] Error: {message}");

        if (txtHeader != null)
            txtHeader.text = $"Video Error";

        if (txtMessage != null)
            txtMessage.text = $"{message}";
    }

    void HandleVideoFrameDropped(VideoPlayer videoPlayer)
    {
        //Debug.LogWarning($"[ErrorHandler][VideoPlayer] Frame dropped");
    }

    public bool IsCurrentError(string header)
    {
        if (messagePanel != null &&
            messagePanel.activeInHierarchy &&
            txtHeader != null &&
            txtHeader.text.Equals(header))
        {
            return true;
        }

        return false;
    }

    public void CloseMessage(string header = null)
    {
        if((!string.IsNullOrEmpty(header) 
            && IsCurrentError(header)) ||
            IsCurrentError(header))
        {
            messagePanel?.SetActive(false);
        }
    }

    public void ShowMessage(string header, string message, float timeoutSecs = -1)
    {
        if (txtHeader != null)
            txtHeader.text = header;

        if (txtMessage != null)
            txtMessage.text = message;

        StartCoroutine(TogglePanel(true, timeoutSecs < 0 ? defaultTimeoutSecs : timeoutSecs));
    }

    IEnumerator TogglePanel(bool show = true, float timeoutSecs = 0)
    {
        if (messagePanel == null)
            yield break;

        if(show)
        {
            messagePanel.SetActive(true);

            if (timeoutSecs > 0)
            {
                yield return new WaitForSeconds(timeoutSecs);
                messagePanel.SetActive(false);
            }
        }
        else
        {
            if (timeoutSecs > 0)
                yield return new WaitForSeconds(timeoutSecs);

            messagePanel.SetActive(false);
        }
    }
}
