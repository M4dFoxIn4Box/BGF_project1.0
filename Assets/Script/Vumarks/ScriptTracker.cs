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
    public int vumarkIdUnlockingBdxGame = -1;
    public int vumarkIdUnlockingBGFGame = -1;
    private int i_current_vumark_index;
    private bool bdxGameStarted = false;
    private bool bGFGameStarted = false;
    public List<Interface_Manager.AppMessages> targetsMessages;
    
    [Header("3D AR Section")]
    public List<GameObject> elementsToSpawn;
    public Transform staticSpawnPoints;
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

    [Header("FeedBackScan")]
    public GameObject feedbackScan;
    public UnityEngine.UI.Image feedbackScanImage;

    [Header("Scriptable Section Quizz")]
    public ScriptableQuizzManager[] quizzLists;
    public List<ScriptableQuizz> quizzAvailable;
    
    [Header("Récompense")]
    public Transform spawnPointReward;
    public Text spawnPointFunFact;
    public GameObject currentReward;
    //private int currentQuizzScore = 0;
    //public int scoreToReach;
    //private bool isAnswered = false;
    //public GameObject currentFakeARObject;

    [Header("ARManager")]
    public bool arIsLock;

    [Header("Sounds")]
    public AudioClip audioQuizzCorrectAnswer;
    public AudioClip audioQuizzBadAnswer;
    public AudioMixerGroup[] mixerGroupQuizz;

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


        if (!SaveManager.Data.eventBGFStarted)
        {
            if (i_current_vumark_index == vumarkIdUnlockingBGFGame)
            {
                Interface_Manager.Instance.StartScanning(i_current_vumark_index);
            }
            else if (i_current_vumark_index != vumarkIdUnlockingBGFGame)
            {
                Interface_Manager.Instance.DisplayMessage(targetsMessages[0]);
            }
        }
        else if (SaveManager.Data.eventBGFStarted)
        {
            Interface_Manager.Instance.StartScanning(i_current_vumark_index);
        }
    }
    
    public void OnTrackerLost()
    {
        Destroy(currentDisplayedElement);
        Interface_Manager.Instance.LostTracker();
    }

    public void ResetScanFeedback ()
    {
        feedbackScan.SetActive(false);
        feedbackScanImage.fillAmount = 0;
    }

    // QUIZZ // answer button section

    public void ARLocker()
    {
        arIsLock = !arIsLock;
    }
    
    public void ActiveAnimation() // Fait apparaître les récompenses liées au VuMark scanné
    {
        if (!SaveManager.Data.eventBGFStarted)
        {
            SaveManager.Data.eventBGFStarted = true;
            SaveManager.SaveToFile();
        }
        
        foreach (var item in mVuMarkManager.GetActiveBehaviours())
        {
            int targetObj = Convert.ToInt32(item.VuMarkTarget.InstanceId.NumericValue);
            Transform vmp = transform.GetChild(targetObj - 1);
            vmp.gameObject.SetActive(true);
            LinkToStaticARElement lts = vmp.GetComponent<LinkToStaticARElement>();
            if (lts != null)
            {
                currentDisplayedElement = Instantiate(elementsToSpawn[targetObj - 1], lts.GetStaticElement().position, lts.GetStaticElement().rotation, lts.GetStaticElement());
                Interface_Manager.Instance.SpotFound(targetObj - 1);
                return;
            }
            currentDisplayedElement = Instantiate(elementsToSpawn[targetObj - 1], vmp.position, vmp.rotation, vmp);
            Interface_Manager.Instance.SpotFound(targetObj - 1);

            //Interface_Manager.Instance.CheckStateButton(targetObj - 1);
        }
    }

    public void RewardButton(int rewardIdx) //Click sur le bouton de la galerie
    {
        currentReward = Instantiate(quizzLists[rewardIdx].rewardToSpawn, spawnPointReward);
        spawnPointFunFact.text = quizzLists[rewardIdx].funFact;
    }

    public void DestroyRewardSpawn()
    {
        Destroy(currentReward);
        Interface_Manager.Instance.HideScore();
    }
    
    void Update()
    {

    }
}