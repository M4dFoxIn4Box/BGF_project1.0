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
    public int vumarkIdUnlockingEventGame = -1;
    private bool eventGameStarted = false;
    public List<TargetsRelatedMessage> targetsMessages;

    [Serializable]
    public class TargetsRelatedMessage
    {
        public string messageName;
        public string messageToDisplay;
        public Sprite imageToDisplay;
    }

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
    //public VuMarkTarget vumark;
    public int vumarkID;

    public int vumarkRewardMinValue;
    
    [Header("Quizz")]
    public int currentErrorCount;
    public bool quizzDone = false;
    public GameObject quizzInterface;
    public GameObject congratulationsImage;
    public List<GameObject> winCountList;
    public List<GameObject> errorCountList;
    public Sprite newErrorImage;
    public GameObject winCounter;
    public GameObject errorCounter;

    public GameObject parentErrorCount;
    public GameObject parentWinCount;
    public Animator quizAnim;
    public AudioSource quizzAudioSource;
    public AudioClip[] audioQuizz;

    //VARIABLES ADD 8 MARS
    public List<bool> b_quizz_is_done;
    public GameObject g_return_to_vumark;
    public GameObject g_try_again;
    public GameObject g_get_first_vumark;
    private int i_current_vumark_index;
    private bool b_quizz_is_active = false;
    private bool b_first_vumark_is_unlock;


    [Header("Fake AR feedback")]

    public GameObject feedBackFakeAR;

    [Header("FeedBackScan")]
    public GameObject feedbackScan;
    public UnityEngine.UI.Image feedbackScanImage;
    public Text loadTextState;
    private bool loadingState = true;
    public float scanDuration;

    [Header("Textes")]
    public Text quizzText;
    private string[] answerString;
    public Text[] answerBoardText;

    [Header("FunFact")]
    public GameObject funFactParent;
    public Text funFactTxt;

    [Header("Boutons")]
    public Button[] buttonList;

    private float r = 0.4509804f;
    private float g = 0.4509804f;
    private float b = 0.4509804f;

    [Header("Scriptable Section Quizz")]
    public ScriptableQuizzManager[] quizzLists;
    public List<ScriptableQuizz> quizzAvailable;

    [Header("Current Scriptables")]
    public ScriptableQuizz currentQuizz;
    public ScriptableQuizzManager currentQuizzList;

    [Header("Récompense")]

    public Transform spawnPointReward;
    public Text spawnPointFunFact;
    public GameObject currentReward;
    private int currentQuizzScore = 0;
    public int scoreToReach;
    private bool isAnswered = false;
    public GameObject currentFakeARObject;

    [Header("ARManager")]
    public bool arIsLock;
    public GameObject mainMenu;

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
            vumarkID = (int)vumark.InstanceId.NumericValue;
            i_current_vumark_index = vumarkID;
        }


        if (!eventGameStarted)
        {
            if (i_current_vumark_index == vumarkIdUnlockingEventGame)
            {
                Interface_Manager.Instance.StartScanning(i_current_vumark_index);
            }
            else if (i_current_vumark_index != vumarkIdUnlockingEventGame)
            {
                Interface_Manager.Instance.DisplayMessage(targetsMessages[0]);
            }
        }
        else if (eventGameStarted)
        {
            Interface_Manager.Instance.StartScanning(i_current_vumark_index);
        }
    }
    
    public void OnTrackerLost()
    {
        Destroy(currentDisplayedElement);
        Interface_Manager.Instance.LostTracker();
        return;

        feedbackScan.SetActive(false);
        loadingState = false;
        feedbackScanImage.fillAmount = 0;

        if (i_current_vumark_index >= vumarkRewardMinValue)
        {

        }
        else
        {
            foreach (var item in mVuMarkManager.GetActiveBehaviours())
            {
                int targetObj = System.Convert.ToInt32(item.VuMarkTarget.InstanceId.NumericValue);
                transform.GetChild(targetObj - 1).gameObject.SetActive(false);
            }
        }

        //LeaveQuizz();
    }

    public void ResetScanFeedback ()
    {
        feedbackScan.SetActive(false);
        feedbackScanImage.fillAmount = 0;
    }
    
    void ScanIsDone()
    {
        foreach (VuMarkTarget vumark in TrackerManager.Instance.GetStateManager().GetVuMarkManager().GetActiveVuMarks())
        {
            vumarkID = (int)vumark.InstanceId.NumericValue;
            i_current_vumark_index = vumarkID;
        }

        if (b_first_vumark_is_unlock == true)
        {
            if (b_quizz_is_done[vumarkID - 1] == false)
            {
                if (vumarkID >= vumarkRewardMinValue) //Pour ouvrir un coffre (stand bgf)
                {
                    int idxToCast = vumarkID - vumarkRewardMinValue;
                    Interface_Manager.Instance.RewardBoxOpened(idxToCast);
                }
                else
                {
                    quizzDone = false;
                    QuizzDisplaying();
                }
            }

            else if (b_quizz_is_done[vumarkID - 1] == true)
            {
                ActiveAnimation();
            }
        }
        else if (i_current_vumark_index != 10)
        {
            g_get_first_vumark.SetActive(true);
        }
        else if(i_current_vumark_index == 10 && b_first_vumark_is_unlock == false)
        {
            QuizzDisplaying();
        }
    }

    public void QuizzDisplaying ()
    {
        quizAnim.SetBool("Erreur", false);

        b_quizz_is_active = true;

            if (quizzDone == false)
            {
                currentQuizzList = quizzLists[vumarkID - 1];
                scoreToReach = quizzLists[vumarkID - 1].scoreToWin;

                        for (int i = 0; i < scoreToReach; i++)
                        {
                            Instantiate(winCounter, parentWinCount.transform);
                            winCountList.Add(winCounter);
                            Debug.Log(winCountList);
                        }

                        for (int i = 0; i < currentQuizzList.errorLimit; i++)
                        {
                            Instantiate(errorCounter, parentErrorCount.transform);
                            errorCountList.Add(errorCounter);
                        }

                quizzAvailable.AddRange(currentQuizzList.scriptableQuizzList);
                quizzDone = true;
            }


            if (quizzAvailable.Count == 0)
            {
                LeaveQuizz();
            }
            else
            {

                currentQuizz = quizzAvailable[(UnityEngine.Random.Range(0, quizzAvailable.Count))];

                quizzInterface.SetActive(true);

                quizzText.text = currentQuizz.quizzQuestion;

                // QUIZZ INITIALISATION

                for (int i = 0; i < buttonList.Length; i++)
                {
                    buttonList[i].interactable = true;
                    answerBoardText[i].text = currentQuizz.answerList[i];
                    buttonList[i].GetComponent<UnityEngine.UI.Image>().color = Color.white;
                }
            }                            
    }

    public void FakeARToDeactivate(GameObject fakeARObject)
    {
        currentFakeARObject = fakeARObject;
    }

    // QUIZZ // answer button section

    public void ARLocker()
    {
        arIsLock = !arIsLock;
    }

    public void ButtonClick (int buttonIdx) //Lier la fonction au bouton de réponse au quizz
    {
        if (buttonIdx + 1 == currentQuizz.rightAnswer)
        {
            buttonList[buttonIdx].GetComponent<UnityEngine.UI.Image>().color = Color.green;
            RightAnswer();
        }
        else
        {
            buttonList[buttonIdx].GetComponent<UnityEngine.UI.Image>().color = Color.red;
            BadAnswer();
        }
    }

    public void LeaveQuizz()
    {
        quizzAvailable.Clear();
        b_quizz_is_active = false;

        if (feedBackFakeAR)
        {
            feedBackFakeAR.SetActive(false);
        }

        for (int i = 0; i < winCountList.Count; i++)
        {
            Destroy(parentWinCount.transform.GetChild(i).gameObject);  
        }

        for (int i = 0; i < errorCountList.Count; i++)
        {
            Destroy(parentErrorCount.transform.GetChild(i).gameObject);
        }

        errorCountList.Clear();
        winCountList.Clear();

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
            buttonList[i].interactable = true;
        }

        quizzInterface.SetActive(false);
        currentErrorCount = 0;
        currentQuizzScore = 0;

        if (congratulationsImage)
        {
            congratulationsImage.SetActive(false);
        }

        if(currentFakeARObject != null)
        {
            Destroy(currentFakeARObject);
        }
        
        if(arIsLock)
        {
            Interface_Manager.Instance.CloseARCamera();
        }
    }

    public void BadAnswer()
    {
        quizAnim.SetBool("Erreur", true);
        AudioManager.s_Singleton.PlaySFX(audioQuizzBadAnswer);
        AudioManager.s_Singleton.GetComponent<AudioSource>().outputAudioMixerGroup = mixerGroupQuizz[0];
        parentErrorCount.transform.GetChild(currentErrorCount).GetComponent<UnityEngine.UI.Image>().sprite = newErrorImage;
        currentErrorCount++;        

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].interactable = false;
        }

        StartCoroutine(TimeBeforeNextQuizz());
    }

    public void RightAnswer()
    {
        AudioManager.s_Singleton.PlaySFX(audioQuizzCorrectAnswer);
        AudioManager.s_Singleton.GetComponent<AudioSource>().outputAudioMixerGroup = mixerGroupQuizz[0];

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].interactable = false;
        }

        parentWinCount.transform.GetChild(currentQuizzScore).GetComponent<UnityEngine.UI.Image>().color = Color.green;
        currentQuizzScore++;

        if (currentQuizzScore == scoreToReach)
        {
            congratulationsImage.SetActive(true);
            currentQuizzScore = 0;
            StartCoroutine(GameWon());
        }
        else if (currentQuizzScore < scoreToReach)
        {
            StartCoroutine(TimeBeforeNextQuizz());
        }
    }

    IEnumerator TimeBeforeNextQuizz ()
    {
        yield return new WaitForSeconds(1);
        if (currentErrorCount == currentQuizzList.errorLimit)
        {
            yield return new WaitForSeconds(1);
            LeaveQuizz();
            g_try_again.SetActive(true);

        }
        currentQuizz = null;    
        QuizzDisplaying();
    }

    IEnumerator GameWon()
    {
        yield return new WaitForSeconds(2);
        // Reset du quizz

        congratulationsImage.SetActive(false);
        quizzInterface.SetActive(false);       
        quizzDone = false;

        if(i_current_vumark_index == 1 && b_first_vumark_is_unlock == false)
        {
            b_first_vumark_is_unlock = true;
        }

        QuizzIsDone();
        LeaveQuizz();
        ReturnToVumark();
    }

    private void QuizzIsDone()
    {
        b_quizz_is_done[i_current_vumark_index-1] = true;
    }

    private void ReturnToVumark()
    {
        g_return_to_vumark.SetActive(true);
    }

    public void ActiveAnimation() // Fait apparaître les récompenses liés au VuMark scanné
    {
        if (!eventGameStarted)
        {
            eventGameStarted = true;
        }
        
        foreach (var item in mVuMarkManager.GetActiveBehaviours())
        {
            int targetObj = System.Convert.ToInt32(item.VuMarkTarget.InstanceId.NumericValue);
            Transform vmp = transform.GetChild(targetObj - 1);
            vmp.gameObject.SetActive(true);
            LinkToStaticARElement lts = vmp.GetComponent<LinkToStaticARElement>();
            if (lts != null)
            {
                currentDisplayedElement = Instantiate(elementsToSpawn[targetObj - 1], lts.GetStaticElement().position, lts.GetStaticElement().rotation, lts.GetStaticElement());
                return;
            }
            currentDisplayedElement = Instantiate(elementsToSpawn[targetObj - 1], vmp.position, vmp.rotation, vmp);
            
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