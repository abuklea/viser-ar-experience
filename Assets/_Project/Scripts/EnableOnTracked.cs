
using UnityEngine;
using Vuforia;

public class EnableOnTracked : DefaultObserverEventHandler
{
    public GameObject objectToEnable;

    ObserverBehaviour observerBehaviour;
    bool isDetected = false;

    protected override void Start()
    {
        base.Start();

        observerBehaviour = GetComponent<ObserverBehaviour>();
        //if (trackableBehaviour)
        //    trackableBehaviour.RegisterOnTrackableStatusChanged(OnTrackableStateChanged);
    }

    //void OnDisable()
    //{
    //    //if (trackableBehaviour)
    //    //    trackableBehaviour.UnregisterOnTrackableStatusChanged(OnTrackableStateChanged);
    //}

    protected override void HandleTargetStatusChanged(Status previousStatus, Status newStatus)
    {
        base.HandleTargetStatusChanged(previousStatus, newStatus);

        if (!isDetected && (newStatus != Status.NO_POSE ||
            newStatus == Status.TRACKED ||
            newStatus == Status.EXTENDED_TRACKED))
        {
            Debug.Log($"Trackable {observerBehaviour.TargetName} found ({newStatus})");

            isDetected = true;

            objectToEnable.SetActive(true);
        }
        else if ((previousStatus == Status.TRACKED || previousStatus == Status.EXTENDED_TRACKED) &&
                 newStatus == Status.NO_POSE)
        {
            Debug.Log($"Trackable {observerBehaviour.TargetName} lost ({newStatus})");

            isDetected = false;

            objectToEnable.SetActive(false);
        }
    }

    //public void OnTrackableStateChanged(TargetStatus status)
    //{
    //    Debug.Log($"OnTrackableStateChanged: {status.NewStatus} [OLD: {status.PreviousStatus}]");

    //    if (!isDetected && (status.NewStatus == TrackableBehaviour.Status.DETECTED ||
    //        status.NewStatus == TrackableBehaviour.Status.TRACKED ||
    //        status.NewStatus == TrackableBehaviour.Status.EXTENDED_TRACKED))
    //    {
    //        Debug.Log($"Trackable {trackableBehaviour.TrackableName} found ({status.NewStatus})");

    //        isDetected = true;

    //        objectToEnable.SetActive(true);
    //    }
    //    else if (status.PreviousStatus == TrackableBehaviour.Status.TRACKED &&
    //             status.NewStatus == TrackableBehaviour.Status.NO_POSE)
    //    {
    //        Debug.Log($"Trackable {trackableBehaviour.TrackableName} lost ({status.NewStatus})");

    //        isDetected = false;

    //        objectToEnable.SetActive(false);
    //    }
    //}
}