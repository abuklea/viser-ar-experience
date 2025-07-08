
using UnityEngine;

public class AppManager : Singleton<MonoBehaviour>
{
    //public bool pause = false;
    public bool isPaused = false;

    void Update()
    {
        if (!isPaused && Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //if(pause && !isPaused)
        //{
        //    Pause();
        //}
        //else if(!pause && isPaused)
        //{
        //    Resume();
        //}
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void Pause()
    {
        //Time.timeScale = 0;
        isPaused = true;
    }

    public void Resume()
    {
        //Time.timeScale = 1;
        isPaused = false;
    }

    private void OnDestroy()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        AndroidJavaClass process = new AndroidJavaClass("android.os.Process");
        int pid = process.CallStatic<int>("myPid");
        process.CallStatic("killProcess", pid);
#endif
    }
}
