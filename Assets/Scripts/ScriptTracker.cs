﻿using System.Collections;
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
    public Text answer1;
    public Text answer2;
    public Text answer3;
    public Text answer4;
    public Text funFact;

    [Header("Boutons")]
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button leaveCanvas;

    private float r = 0.4509804f;
    private float g = 0.4509804f;
    private float b = 0.4509804f;

    private string answer1Text;
    private string answer2Text;
    private string answer3Text;
    private string answer4Text;

    [Header("Scriptable Section Quizz")]
    public ScriptableQuizzManager[] quizzLists;

    public List<ScriptableQuizz> quizzAvailable;
    //public ScriptableQuizz[] scriptableQuizzList;

    [Header("Current Scriptables")]
    public ScriptableQuizz currentScriptableQuizz;
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


      
    }

    public void QuizzDisplaying ()
    {
        currentQuizzList = quizzLists[vumarkID - 1];

        if (!quizzDone)
        {
            quizzAvailable.AddRange(currentQuizzList.scriptableQuizzList);
        }

        currentScriptableQuizz = currentQuizzList.scriptableQuizzList[(Random.Range(0, currentQuizzList.scriptableQuizzList.Count))];

        quizzDone = true;

                button1.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
                button2.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
                button3.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
                button4.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);

                button1.interactable = true;
                button2.interactable = true;
                button3.interactable = true;
                button4.interactable = true;

                quizzInterface.SetActive(true);

                quizzText.text = currentScriptableQuizz.quizzQuestion;
                answer1Text = currentScriptableQuizz.answer1;
                answer2Text = currentScriptableQuizz.answer2;
                answer3Text = currentScriptableQuizz.answer3;
                answer4Text = currentScriptableQuizz.answer4;

                answer1.text = answer1Text;
                answer2.text = answer2Text;
                answer3.text = answer3Text;
                answer4.text = answer4Text;

                button1.onClick.AddListener(TaskOnClick1);
                button2.onClick.AddListener(TaskOnClick2);
                button3.onClick.AddListener(TaskOnClick3);
                button4.onClick.AddListener(TaskOnClick4);

                leaveCanvas.onClick.AddListener(LeaveQuizz);

        quizzAvailable.Remove(currentScriptableQuizz);

        if (quizzAvailable.Count == 0)
        {
            Debug.Log("List is empty");
            LeaveQuizz();
        }
    }

    // QUIZZ // answer button section

    public void TaskOnClick1() // BOUTON 1
    {
        if (currentScriptableQuizz.rightAnswer == 1)
        {
            button1.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            button2.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            button3.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            button4.GetComponent<UnityEngine.UI.Image>().color = Color.red;

            RightAnswer();
        }
        else
        {
            BadAnswer();
            button1.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            button1.interactable = false;
        }
    }

    public void TaskOnClick2() // BOUTON 2
    {
        if (currentScriptableQuizz.rightAnswer == 2)
        {
            button1.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            button2.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            button3.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            button4.GetComponent<UnityEngine.UI.Image>().color = Color.red;

            RightAnswer();
        }
        else
        {
            BadAnswer();
            button2.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            button2.interactable = false;
        }
    }

    public void TaskOnClick3() // BOUTON 3
    {
        if (currentScriptableQuizz.rightAnswer == 3)
        {
            button1.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            button2.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            button3.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            button4.GetComponent<UnityEngine.UI.Image>().color = Color.red;

            RightAnswer();
        }
        else
        {
            BadAnswer();
            button3.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            button3.interactable = false;
        }
    }

    public void TaskOnClick4() // BOUTON 4
    {
        if (currentScriptableQuizz.rightAnswer == 4)
        {
            button1.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            button2.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            button3.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            button4.GetComponent<UnityEngine.UI.Image>().color = Color.green;

            RightAnswer();
        }
        else
        {
            BadAnswer();
            button4.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            button4.interactable = false;
        }
    }

    public void LeaveQuizz()
    {
        quizzAvailable.Clear();

        button1.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
        button2.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
        button3.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
        button4.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);

        button1.interactable = true;
        button2.interactable = true;
        button3.interactable = true;
        button4.interactable = true;


        quizzInterface.SetActive(false);
        currentQuizzScore = 0;
        quizzDone = false;
       
        if (congratulationsImage)
        {
            congratulationsImage.SetActive(false);
        }

        currentQuizzList = null;
        currentScriptableQuizz = null;

    }

    public void BadAnswer()
    {
        button1.interactable = false;
        button2.interactable = false;
        button3.interactable = false;
        button4.interactable = false;

        StartCoroutine(TimeBeforeNextQuizz());
    }

    public void RightAnswer()
    {
        button1.interactable = false;
        button2.interactable = false;
        button3.interactable = false;
        button4.interactable = false;

        currentQuizzScore++;

        if(currentQuizzScore == scoreToReach)
        {
            congratulationsImage.SetActive(true);
            Debug.Log("You Win");
            StartCoroutine(GameWon());
        }
        else if (currentQuizzScore < scoreToReach)
        {
            StartCoroutine(TimeBeforeNextQuizz());
        }

     
    }

    IEnumerator TimeBeforeNextQuizz ()
    {
        yield return new WaitForSeconds(2);

        QuizzDisplaying();
    }

    IEnumerator GameWon()
    {
        yield return new WaitForSeconds(2);

        // Reset du quizz
        screenShare.SetActive(true);
        congratulationsImage.SetActive(false);
        quizzInterface.SetActive(false);
        currentQuizzScore = 0;
        quizzDone = false; ;

        // IMPORTANT !!! //

        // A décommenter pour la version cliente 

        //if (currentScriptableQuizz)
        //{
        //    currentScriptableQuizz.hasBeenDone = true;
        //}
        //else if (currentScriptableMiniGame)
        //{
        //    currentScriptableMiniGame.hasBeenDone = true;
        //}


        // Fais apparaître les récompenses liés au VuMark scanné

        foreach (var item in mVuMarkManager.GetActiveBehaviours())
        {
            int targetObj = System.Convert.ToInt32(item.VuMarkTarget.InstanceId.NumericValue);
            transform.GetChild(targetObj - 1).gameObject.SetActive(true);
            Interface_Manager.Instance.CheckStateButton(targetObj - 1);
        }

    }

}