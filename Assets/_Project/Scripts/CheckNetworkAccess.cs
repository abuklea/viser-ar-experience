
using System;
using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;

public class CheckNetworkAccess : MonoBehaviour
{
    public string checkHTML_URL = "http://google.com";
    public ErrorHandler errorHandler;
    public int numChecks = 0;
    public float checkDelaySecs = 5f;
    public int numCharactersToCheck = 10;

    public string networkAccessErrorText = "No Internet Access";
    public string networkAccessErrorText2 = "Please check your internet access";

    string html = string.Empty;
    bool requestInProgress = false;
    //HttpWebRequest webRequest;
    bool connectSuccess = false;

    void Start()
    {
        StartCoroutine(DoNetworkChecks());
    }

    IEnumerator DoNetworkChecks()
    {
        int checksPerformed = 0;

        while(numChecks <= 0 || (numChecks > 0 && checksPerformed < numChecks))
        {
            yield return StartCoroutine(CheckAccess());

            //yield return new WaitWhile(() => requestInProgress == true);

            ++checksPerformed;

            if (connectSuccess)
            {
                Debug.Log($"[CheckNetworkAccess] Network available! URL: {checkHTML_URL}");

                if(errorHandler.IsCurrentError(networkAccessErrorText))
                    errorHandler.CloseMessage(networkAccessErrorText);
            }
            else
            {
                Debug.LogWarning($"[CheckNetworkAccess] Error: No network access to URL: {checkHTML_URL}");

                if (!errorHandler.IsCurrentError(networkAccessErrorText))
                    errorHandler.ShowMessage(networkAccessErrorText, networkAccessErrorText2, 0);
            }

            yield return new WaitForSeconds(checkDelaySecs);
        }
    }

    IEnumerator CheckAccess()
    {
        requestInProgress = true;

        StartCoroutine(GetHtmlFromUri(checkHTML_URL));

        yield return new WaitWhile(() => requestInProgress == true);

        if (!string.IsNullOrEmpty(html))
            connectSuccess = true;
        else
            connectSuccess = false;

        yield return null;
    }

    IEnumerator GetHtmlFromUri(string resource)
    {
        html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        char[] cs = new char[numCharactersToCheck];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            html = string.Empty;
        }

        requestInProgress = false;

        yield return null;
    }
}
