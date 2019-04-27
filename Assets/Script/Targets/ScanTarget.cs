using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System;

public class ScanTarget : MonoBehaviour, ITrackableEventHandler
{
    
    private TrackableBehaviour mTrackableBehaviour;
    
    // Start is called before the first frame update
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour != null)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            if (Interface_Manager.Instance.CanScan())
            {
                RAManager.s_Singleton.TargetScanned(transform.GetSiblingIndex());
            }
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED && newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            RAManager.s_Singleton.DestroyAnimation();
        }
    }
}
