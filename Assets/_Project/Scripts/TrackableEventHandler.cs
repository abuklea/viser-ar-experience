
using UnityEngine;
//using UnityEngine.Analytics;
using Vuforia;

public class TrackableEventHandler : DefaultObserverEventHandler
{
    public string trackableID;
    ObserverBehaviour observerBehaviour;
    bool isDetected = false;
    float startTrackedSecs = 0;

    //static int detectedCount = 0;

    protected override void Start()
    {
        base.Start();

        observerBehaviour = GetComponent<ObserverBehaviour>();
        //if (trackableBehaviour)
        //    trackableBehaviour.RegisterTrackableEventHandler(this);
    }

    //void OnDisable()
    //{
    //    //if (trackableBehaviour)
    //    //    trackableBehaviour.UnregisterTrackableEventHandler(this);
    //}

    public void AnalyticsTrackableDetected()
    {
        startTrackedSecs = Time.time;
        //AnalyticsEvent.Custom("trackable_detected", new Dictionary<string, object>
        //{
        //    { "user_id", AnalyticsSessionInfo.userId},
        //    { "session_id", AnalyticsSessionInfo.sessionId},
        //    { "trackable_ID",  trackableID},
        //    { "trackable_name",  trackableBehaviour.TrackableName},
        //    { "detected_count", ++detectedCount},
        //    { "secs_since_app_start", Time.timeSinceLevelLoad }
        //});
    }

    public void AnalyticsTrackableLost()
    {
        //AnalyticsEvent.Custom("trackable_lost", new Dictionary<string, object>
        //{
        //    { "user_id", AnalyticsSessionInfo.userId},
        //    { "session_id", AnalyticsSessionInfo.sessionId},
        //    { "trackable_ID",  trackableID},
        //    { "trackable_name",  trackableBehaviour.TrackableName},
        //    { "detected_count", detectedCount},
        //    { "secs_tracked", (Time.time - startTrackedSecs) }
        //});
    }

    protected override void HandleTargetStatusChanged(Status previousStatus, Status newStatus)
    {
        base.HandleTargetStatusChanged(previousStatus, newStatus);

        if (!isDetected && (newStatus != Status.NO_POSE ||
            newStatus == Status.TRACKED ||
            newStatus == Status.EXTENDED_TRACKED))
        {
            //Debug.Log($"Trackable {observerBehaviour.TargetName} found ({newStatus})");

            AnalyticsTrackableDetected();

            isDetected = true;
        }
        else if ((previousStatus == Status.TRACKED || previousStatus == Status.EXTENDED_TRACKED) &&
                 newStatus == Status.NO_POSE)
        {
            //Debug.Log($"Trackable {trackableBehaviour.TrackableName} lost ({newStatus})");

            AnalyticsTrackableLost();

            isDetected = false;
        }
    }

    //public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    //{
    //    if (!isDetected && (newStatus == TrackableBehaviour.Status.DETECTED ||
    //        newStatus == TrackableBehaviour.Status.TRACKED ||
    //        newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED))
    //    {
    //        Debug.Log($"Trackable {trackableBehaviour.TrackableName} found ({newStatus})");

    //        AnalyticsTrackableDetected();

    //        isDetected = true;
    //    }
    //    else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
    //             newStatus == TrackableBehaviour.Status.NO_POSE)
    //    {
    //        //Debug.Log($"Trackable {trackableBehaviour.TrackableName} lost ({newStatus})");

    //        AnalyticsTrackableLost();

    //        isDetected = false;
    //    }
    //}
}