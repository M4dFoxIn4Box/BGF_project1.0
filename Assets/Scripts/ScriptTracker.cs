using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ScriptTracker : MonoBehaviour, ITrackableEventHandler

{
    TrackableBehaviour mTrackableBehaviour;
    VuMarkManager mVuMarkManager;

    public GameObject ChambouleTout;
    public Transform spawnPoint;
    public GameObject teapot;

    public VuMarkTarget vumark;

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour != null)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        mVuMarkManager = TrackerManager.Instance.GetStateManager().GetVuMarkManager();

    }

    // Update is called once per frame
    void Update()
    {
   
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackerFound();
            Instantiate(ChambouleTout, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), Quaternion.identity);
            
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED && newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            OnTrackerLost();
        }
    }

    void OnTrackerFound()
    {
        /*foreach (var bhvr in mVuMarkManager.GetActiveBehaviours())
        {
            //int targetObj = System.Convert.ToInt32 (item.VuMarkTarget.InstanceId.NumericValue);
          //  transform.GetChild(targetObj - 1).gameObject.SetActive(true);
          //  UI_Manager.Instance.FillInScanIdx(targetObj-1);

            vumark = bhvr.VuMarkTarget;

        }*/
        foreach (VuMarkTarget vumark in TrackerManager.Instance.GetStateManager().GetVuMarkManager().GetActiveVuMarks())
        {
            if (vumark.InstanceId.NumericValue == 2)
            {

                teapot.SetActive(true);
                Debug.Log("ID1");

            }
        }




    }
    

    void OnTrackerLost()
    {
        foreach (var item in mVuMarkManager.GetActiveBehaviours())
        {
            int targetObj = System.Convert.ToInt32 (item.VuMarkTarget.InstanceId.NumericValue);
            transform.GetChild(targetObj - 1).gameObject.SetActive(false);
        }
    }
}