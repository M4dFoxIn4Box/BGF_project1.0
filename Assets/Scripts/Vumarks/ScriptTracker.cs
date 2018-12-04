using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ScriptTracker : MonoBehaviour, ITrackableEventHandler

{
    public GameObject screenShare;
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
    public VuMarkTarget vumark;
    public int vumarkID;

    public int vumarkRewardMinValue;

    [Header("Tutoriel")]
    public bool firstScan = true;

    [Header("Quizz")]
    //public Text errorCountTxt;
    public int currentErrorCount;    
    //public Text scoreDisplay;
    public bool quizzDone = false;
    public GameObject quizzInterface;
    //public GameObject errorImage;
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

    [Header("FeedBackScan")]
    public float timer = 0f;
    public GameObject imageFillAmount;
    public UnityEngine.UI.Image feedbackScan;
    private bool coolingDown = false;

    [Header("Textes")]
    public Text quizzText;
    private string[] answerString;
    public Text[] answerBoardText;

    [Header("FunFact")]
    public GameObject funFactParent;
    public Text funFactTxt;

    [Header("Boutons")]
    public Button[] buttonList;

    public Button leaveCanvas;

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
    private GameObject currentFakeARObject;

    [Header("ARManager")]
    public bool arIsLock;
    public GameObject mainMenu;

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
        imageFillAmount.SetActive(true);
        coolingDown = true;
    }
    
    public void OnTrackerLost()
    {
        imageFillAmount.SetActive(false);
        timer = 0;
        coolingDown = false;
        feedbackScan.fillAmount = 1;


        if (vumarkID >= vumarkRewardMinValue)
        {
            Debug.Log("This is correct bobby");
        }
        else
        {
            foreach (var item in mVuMarkManager.GetActiveBehaviours())
            {
                int targetObj = System.Convert.ToInt32(item.VuMarkTarget.InstanceId.NumericValue);
                transform.GetChild(targetObj - 1).gameObject.SetActive(false);
            }
        }

        LeaveQuizz();
        Debug.Log(currentQuizzList);
    }

    void TrackerFound()
    {
        foreach (VuMarkTarget vumark in TrackerManager.Instance.GetStateManager().GetVuMarkManager().GetActiveVuMarks())
        {
            vumarkID = (int)vumark.InstanceId.NumericValue;
        }

        if (vumarkID >= vumarkRewardMinValue)
        {
            int idxToCast = vumarkID - vumarkRewardMinValue;
            Interface_Manager.Instance.RewardBoxOpened(idxToCast);
            Debug.Log("ID cast = " + idxToCast);
        }
        else
        {
            quizzDone = false;
            QuizzDisplaying();
        }
    }

    public void QuizzDisplaying ()
    {
        quizAnim.SetBool("Erreur", false);
            if (quizzDone == false)
            {
                currentQuizzList = quizzLists[vumarkID - 1];
                scoreToReach = quizzLists[vumarkID - 1].scoreToWin;

            Debug.Log(scoreToReach);

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

                currentQuizz = quizzAvailable[(Random.Range(0, quizzAvailable.Count))];

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
        Debug.Log(arIsLock);
    }

    public void ButtonClick (int buttonIdx)
    {
        if(buttonIdx + 1 == currentQuizz.rightAnswer)
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

        if (screenShare)
        {
            screenShare.SetActive(false);
        }

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
        quizzAudioSource.PlayOneShot(audioQuizz[0]);
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

        quizzAudioSource.PlayOneShot(audioQuizz[1]);
        quizzAvailable.Remove(currentQuizz);
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].interactable = false;
        }

        parentWinCount.transform.GetChild(currentQuizzScore).GetComponent<UnityEngine.UI.Image>().color = Color.green;
        currentQuizzScore++;
        Debug.Log(currentQuizzScore);

        //scoreDisplay.text = "SCORE" + " " + currentQuizzScore + " / " + scoreToReach;

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
            //errorImage.SetActive(true);
            yield return new WaitForSeconds(1);
            LeaveQuizz();
        }
        currentQuizz = null;    
        QuizzDisplaying();
    }

    IEnumerator GameWon()
    {
        yield return new WaitForSeconds(2);

        // Reset du quizz
      
 
        screenShare.SetActive(true);
        congratulationsImage.SetActive(false);
        quizzInterface.SetActive(false);       
        quizzDone = false;

        // Fais apparaître les récompenses liés au VuMark scanné

        foreach (var item in mVuMarkManager.GetActiveBehaviours())
        {
            int targetObj = System.Convert.ToInt32(item.VuMarkTarget.InstanceId.NumericValue);
            transform.GetChild(targetObj - 1).gameObject.SetActive(true);
            Interface_Manager.Instance.CheckStateButton(targetObj - 1);
        }

    }

    public void RewardButton(int rewardIdx) //Click sur le bouton de la galerie
    {
        currentReward = Instantiate(quizzLists[rewardIdx].rewardToSpawn, spawnPointReward);
        spawnPointFunFact.text = quizzLists[rewardIdx].funFact;
        Debug.Log(transform.GetSiblingIndex());
    }

    public void DestroyRewardSpawn()
    {
        Destroy(currentReward);
    }


    void fillAmount()
    {        
        if (coolingDown == true)
        {
            timer += Time.deltaTime;
            feedbackScan.fillAmount -= 0.2f  * Time.deltaTime;
        }

        if (timer >= 5)
        {
            TrackerFound();
            imageFillAmount.SetActive(false);
            coolingDown = false;
            timer = 0;
            feedbackScan.fillAmount = 1;
        }
    }

    void Update()
    {
        fillAmount();
    }


}