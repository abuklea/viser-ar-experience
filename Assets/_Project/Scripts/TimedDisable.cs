
using System.Collections;
using TMPro;
using UltEvents;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimedDisable : MonoBehaviour, IPointerDownHandler
{
    //public CanvasGroup group;
    public float disableAfterSecs = 1f;
    public float fadeOutSecs = 1f;
    public GameObject objectToDisable;
    public bool paused = false;
    
    public UltEvent Finished;

    private Image[] images;
    private Text[] texts;
    private TMP_Text[] tmpTexts;
    private Coroutine startCo;
    private Coroutine fadeCo;

    private void Awake()
    {
        if (objectToDisable != null)
        {
            images = objectToDisable.GetComponentsInChildren<Image>();
            texts = objectToDisable.GetComponentsInChildren<Text>();
            tmpTexts = objectToDisable.GetComponentsInChildren<TMP_Text>();
        }
    }

    void Start()
    {
        startCo = StartCoroutine(StartTimer());

        //if (group == null)
        //    group = GetComponent<CanvasGroup>();
    }

    public void Pause(bool pause = true)
    {
        paused = pause;
    }
    
    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(disableAfterSecs);

        if(fadeCo == null)
            fadeCo = StartCoroutine(DoFadeOut());
    }

    IEnumerator DoFadeOut()
    {
        if (objectToDisable != null)
        {
            float elapsedTime = 0.0f;
            float alpha = 1f;

            while (paused || elapsedTime < fadeOutSecs)
            {
                yield return null;

                if (paused)
                    continue;
                    
                elapsedTime += Time.deltaTime;
                alpha = 1.0f - Mathf.Clamp01(elapsedTime / fadeOutSecs);

                //group.alpha = alpha;

                foreach (Image img in images)
                {
                    Color c = img.color;
                    c.a = alpha;
                    img.color = c;
                }

                foreach (Text txt in texts)
                {
                    Color c = txt.color;
                    c.a = alpha;
                    txt.color = c;
                }
                
                foreach (TMP_Text txt in tmpTexts)
                {
                    txt.alpha = alpha;
                }
            }
            
            Finished?.Invoke();

            objectToDisable.SetActive(false);
        }

        startCo = null;
        fadeCo = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (fadeCo == null)
        {
            if (startCo != null)
                StopCoroutine(startCo);

            fadeCo = StartCoroutine(DoFadeOut());
        }
    }
}
