using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ScriptTracker : MonoBehaviour, ITrackableEventHandler

{
    TrackableBehaviour mTrackableBehaviour;
    VuMarkManager mVuMarkManager;

    public GameObject ChambouleTout;
    public Transform spawnPoint;
    //public GameObject teapot;

    public VuMarkTarget vumark;

    [Header("Mini Game")]
    public ulong miniGameLimit;

    [Header("Scan")]
    public ulong scanLimit;

    [Header("Quizz")]
    public ulong quizzLimit;
    //public UnityEngine.UI.Image questionBackground;
    //public UnityEngine.UI.Image screenBackground;

    public GameObject quizzInterface;
    public GameObject quizzOnlyPanel;
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
    private string funFactText;

    [Header("Récompense")]
   
    private bool isAnswered = false;

    [Header("Scriptable n° Quizz")]
    public ScriptableQuizz currentScriptableQuizz;

    // Tableau

    [Header("Quizz Section")]
    public ScriptableQuizz[] scriptableQuizzList;

        [Header("Mini Game Section")]
    public ScriptableMiniGame[] scriptableMiniGameClass;

        [Header("Scan Section")]
    public ScriptableScan[] scriptableScanClass;

    //QUIZZ


    void Start()
    {
        
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour != null)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        mVuMarkManager = TrackerManager.Instance.GetStateManager().GetVuMarkManager();

        //QUIZZ

        //QUIZZ


    }

    // Update is called once per frame
    void Update()
    {
        //QUIZZ
        if (Input.GetKeyDown("q"))
        {
            //screenBackground.enabled = true;
        }
        //QUIZZ
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
            if (vumark.InstanceId.NumericValue <= quizzLimit)
            {
                button1.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
                button2.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
                button3.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
                button4.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);

                button1.interactable = true;
                button2.interactable = true;
                button3.interactable = true;
                button4.interactable = true;

                quizzInterface.SetActive(true);
                currentScriptableQuizz = scriptableQuizzList[vumark.InstanceId.NumericValue - 1];
                Debug.Log(vumark.InstanceId.NumericValue);
                quizzText.text = currentScriptableQuizz.quizzQuestion;
                answer1Text = currentScriptableQuizz.answer1;
                answer2Text = currentScriptableQuizz.answer2;
                answer3Text = currentScriptableQuizz.answer3;
                answer4Text = currentScriptableQuizz.answer4;
                funFactText = currentScriptableQuizz.funFact;

                answer1.text = answer1Text;
                answer2.text = answer2Text;
                answer3.text = answer3Text;
                answer4.text = answer4Text;
                funFact.text = funFactText;

                button1.onClick.AddListener(TaskOnClick1);
                button2.onClick.AddListener(TaskOnClick2);
                button3.onClick.AddListener(TaskOnClick3);
                button4.onClick.AddListener(TaskOnClick4);

                leaveCanvas.onClick.AddListener(LeaveQuizz);
                Debug.Log("Test scriptable board");
            }
            else if (vumark.InstanceId.NumericValue <= scanLimit)
            {
                StartCoroutine(GameWon());
            }
            else if(vumark.InstanceId.NumericValue <= miniGameLimit)
            {

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

        quizzInterface.SetActive(false);
    }

    //QUIZZ
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
            button4.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            button4.interactable = false;
        }
    }


    public void LeaveQuizz()
    {

        //if (isAnswered == true)
        //{
        //    quizzOnlyPanel.SetActive(false);
        //    screenBackground.enabled = false;


        //}

        button1.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
        button2.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
        button3.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
        button4.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);

        //questionBackground.color = new Color(0.7254902f, 0.7254902f, 0.7254902f);

        button1.interactable = true;
        button2.interactable = true;
        button3.interactable = true;
        button4.interactable = true;
    }

    public void RightAnswer()
    {
        button1.interactable = false;
        button2.interactable = false;
        button3.interactable = false;
        button4.interactable = false;

        congratulationsImage.SetActive(true);

        isAnswered = true; 

        StartCoroutine(GameWon());

    }

    IEnumerator GameWon()
    {

        yield return new WaitForSeconds(2);
        quizzInterface.SetActive(false);
        foreach (var item in mVuMarkManager.GetActiveBehaviours())
        {
            int targetObj = System.Convert.ToInt32(item.VuMarkTarget.InstanceId.NumericValue);
            transform.GetChild(targetObj - 1).gameObject.SetActive(true);
            // UI_Manager.Instance.FillInScanIdx(targetObj - 1);
        }

    }
    //QUIZZ

}