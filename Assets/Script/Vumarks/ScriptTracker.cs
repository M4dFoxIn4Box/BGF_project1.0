using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using UnityEngine.Audio;
using System;

public class ScriptTracker : MonoBehaviour, ITrackableEventHandler
{

    //CODE YANNICK

    [Header("Event Section")]
    public List<Interface_Manager.AppMessages> targetsMessages;
    private int i_current_vumark_index;
    
    [Header("3D AR Section")]
    private GameObject currentDisplayedElement;

    //END CODE YANNICK

    public static ScriptTracker Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    TrackableBehaviour mTrackableBehaviour;
    VuMarkManager mVuMarkManager;
    
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour != null)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        mVuMarkManager = TrackerManager.Instance.GetStateManager().GetVuMarkManager();
        
    }


    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackerFound();
            
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED && newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            OnTrackerLost();
        }
    }

    void OnTrackerFound()
    {
        foreach (VuMarkTarget vumark in TrackerManager.Instance.GetStateManager().GetVuMarkManager().GetActiveVuMarks())
        {
            i_current_vumark_index = (int)vumark.InstanceId.NumericValue;
        }

        int vumarkIdUnlockingMainGame = Interface_Manager.Instance.GetTargetIdUnlockingMainGame();
        int vumarkIdUnlockingTeaserGame = Interface_Manager.Instance.GetTargetIdUnlockingTeaserGame();

        //Si je scanne la cible qui débloque le Main Event...
        if (i_current_vumark_index == vumarkIdUnlockingMainGame)
        {
            //...et si c'est la première fois, je lock le Teaser Event et je débloque le Main Event
            if (!SaveManager.Data.eventMainStarted)
            {
                SaveManager.UnlockEventMain();
            }
            //Et je peux commencer à scanner dans le Main Event
            Interface_Manager.Instance.StartScanning(i_current_vumark_index);
        }
        //Sinon, si je scanne n'importe quelle autre cible...
        else if (i_current_vumark_index != vumarkIdUnlockingMainGame)
        {
            //...si c'est une cible du Main Event...
            if (i_current_vumark_index < vumarkIdUnlockingTeaserGame)
            {
                //...si je n'ai pas encore scanné la cible qui débloque le Main Event, j'obtiens un message qui m'indique quelle cible scanner à l'accueil
                if (!SaveManager.Data.eventMainStarted)
                {
                    Interface_Manager.Instance.DisplayMessage(targetsMessages[0]);
                }
                //...si j'ai scanné la cible qui débloque le Main Event, je peux la scanner
                else if (SaveManager.Data.eventMainStarted)
                {
                    Interface_Manager.Instance.StartScanning(i_current_vumark_index);
                }
            }
            //...si c'est une cible du Teaser Event...
            else if (i_current_vumark_index >= vumarkIdUnlockingTeaserGame)
            {
                //...si le Teaser Event est locké, j'obtiens un message qui m'indique que seules les cibles du Main Event sont désormais disponibles
                if (SaveManager.Data.eventTeaserLocked)
                {
                    Interface_Manager.Instance.DisplayMessage(targetsMessages[2]);
                }
                //...si le Teaser Event n'est pas bloqué...
                else if (!SaveManager.Data.eventTeaserLocked)
                {
                    //...si je scanne la cible qui débloque le Teaser Event...
                    if (i_current_vumark_index == vumarkIdUnlockingTeaserGame)
                    {
                        //...si le Teaser Event n'a pas encore été débloqué, je le débloque
                        if (!SaveManager.Data.eventTeaserStarted)
                        {
                            SaveManager.UnlockEventTeaser();
                        }
                        //Et je peux commencer à scanner dans le Teaser Event
                        Interface_Manager.Instance.StartScanning(i_current_vumark_index);
                    }
                    //...si je scanne une des cibles du Teaser Event qui ne le débloque pas...
                    else if (i_current_vumark_index > vumarkIdUnlockingTeaserGame)
                    {
                        //...si le Teaser Event n'a pas encore été débloqué, j'obtiens un message qui m'indique quelle cible scanner et où la trouver
                        if (!SaveManager.Data.eventTeaserStarted)
                        {
                            Interface_Manager.Instance.DisplayMessage(targetsMessages[1]);
                        }
                        //...si le Teaser Event a été débloqué, je peux scanner la cible
                        else if (SaveManager.Data.eventTeaserStarted)
                        {
                            Interface_Manager.Instance.StartScanning(i_current_vumark_index);
                        }
                    }
                }
            }
        }
    }
    
    public void OnTrackerLost()
    {
        if (currentDisplayedElement != null)
        {
            Destroy(currentDisplayedElement);
        }
        Interface_Manager.Instance.LostTracker();
    }
    
    public void ActiveAnimation() // Fait apparaître les récompenses liées au VuMark scanné
    {
        foreach (var item in mVuMarkManager.GetActiveBehaviours())
        {
            int targetObj = Convert.ToInt32(item.VuMarkTarget.InstanceId.NumericValue);
            Transform vmp = transform.GetChild(targetObj - 1);
            vmp.gameObject.SetActive(true);
            LinkToStaticARElement lts = vmp.GetComponent<LinkToStaticARElement>();
            if (!SaveManager.Data.artefactsUnlocked[targetObj - 1])
            {
                //SaveManager.Data.badgesUnlocked[targetObj - 1] = true;
                SaveManager.Data.artefactsUnlocked[targetObj - 1] = true;
                SaveManager.SaveToFile();
            }
            if (lts != null)
            {
                currentDisplayedElement = Instantiate(Interface_Manager.Instance.elementsToSpawn[targetObj - 1], lts.GetStaticElement().position, lts.GetStaticElement().rotation, lts.GetStaticElement());
                Interface_Manager.Instance.CompleteChallenge(targetObj - 1);
                return;
            }
            currentDisplayedElement = Instantiate(Interface_Manager.Instance.elementsToSpawn[targetObj - 1], vmp.position, vmp.rotation, vmp);
            Interface_Manager.Instance.CompleteChallenge(targetObj - 1);
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            foreach (VuMarkTarget vumark in TrackerManager.Instance.GetStateManager().GetVuMarkManager().GetActiveVuMarks())
            {
                Debug.Log((int)vumark.InstanceId.NumericValue);
            }
        }
    }
}