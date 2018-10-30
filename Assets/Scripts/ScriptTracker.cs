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
    public ulong vumarkID;

    [Header("Quizz")]
    public bool quizzDone = false;
    public GameObject quizzInterface;
    public GameObject congratulationsImage;

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

    private int currentQuizzScore = 0;
    public int scoreToReach;
    private bool isAnswered = false;
    
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
            vumarkID = vumark.InstanceId.NumericValue;
        }

        quizzDone = false;
        QuizzDisplaying();
    }
    
    public void OnTrackerLost()
    {

        if (screenShare)
        {
            screenShare.SetActive(false);
        }

        foreach (var item in mVuMarkManager.GetActiveBehaviours())
        {
            int targetObj = System.Convert.ToInt32 (item.VuMarkTarget.InstanceId.NumericValue);
            transform.GetChild(targetObj - 1).gameObject.SetActive(false);
        }

        LeaveQuizz();
        Debug.Log(currentQuizzList);
    }

    public void QuizzDisplaying ()
    {
        if (quizzDone == false)
        {
            currentQuizzList = quizzLists[vumarkID - 1];
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
                Debug.Log("Compteur de boucle" + i);
                buttonList[i].interactable = true;
                answerBoardText[i].text = currentQuizz.answerList[i]; 
                buttonList[i].GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);            

            }

            leaveCanvas.onClick.AddListener(LeaveQuizz);
        }              
                
    }

    // QUIZZ // answer button section


    public void ButtonClick (int buttonIdx)
    {
        Debug.Log("And the right answer is... " + currentQuizz.rightAnswer);
        if(buttonIdx + 1 == currentQuizz.rightAnswer)
        {
            buttonList[buttonIdx].GetComponent<UnityEngine.UI.Image>().color = Color.green;
            RightAnswer();
        }
        else
        {
            Debug.Log("BAD ANSWER");
            buttonList[buttonIdx].GetComponent<UnityEngine.UI.Image>().color = Color.red;
            BadAnswer();
        }
    }

    public void LeaveQuizz()
    {
        quizzAvailable.Clear();

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
            buttonList[i].interactable = true;
        }

        funFactParent.SetActive(false);
        
        quizzInterface.SetActive(false);
        currentQuizzScore = 0;

        if (congratulationsImage)
        {
            congratulationsImage.SetActive(false);
        }
        
    }

    public void BadAnswer()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].interactable = false;
        }

        StartCoroutine(TimeBeforeNextQuizz());
    }

    public void RightAnswer()
    {

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].interactable = false;
        }

        currentQuizzScore++;

        Debug.Log("Current Quizz Score = " + currentQuizzScore);

        if (currentQuizzScore == scoreToReach)
        {
            funFactTxt.text = currentQuizzList.funFact;
            funFactParent.SetActive(true);
            congratulationsImage.SetActive(true);
            currentQuizzScore = 0;
            Debug.Log("You're score = " + currentQuizzScore);
            StartCoroutine(GameWon());
        }
        else if (currentQuizzScore < scoreToReach)
        {
            StartCoroutine(TimeBeforeNextQuizz());
        }
    }

    IEnumerator TimeBeforeNextQuizz ()
    {
        quizzAvailable.Remove(currentQuizz);
        yield return new WaitForSeconds(1);
        currentQuizz = null;
        yield return new WaitForSeconds(1);
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

}