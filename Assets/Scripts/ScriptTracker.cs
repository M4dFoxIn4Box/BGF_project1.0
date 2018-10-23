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

    [Header("Game Limits")]
    public ulong miniGameLimit; // est égale à la somme de quizz et de mini jeux présent
    public ulong quizzLimit; // est égale au nombre de quizz présent


    [Header("Mini Game")]    
    public Transform miniGameSpawnPoint;
    public GameObject miniGameToDestroy;
    //public int currentScore;
    //public int scoreToReach;

    [Header("Quizz")]
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
    private string funFactText;

    [Header("Scriptable Section Quizz")]   
    public ScriptableQuizz[] scriptableQuizzList;

    [Header("Scriptable Section Mini Game")]    
    public ScriptableMiniGame[] scriptableMiniGameList;

    [Header("Scriptable Section Scan")]   
    public ScriptableScan[] scriptableScanList;

    [Header("Current Scriptables")]
    public ScriptableQuizz currentScriptableQuizz;
    public ScriptableMiniGame currentScriptableMiniGame;
    public ScriptableScan currentScriptableScan;

    [Header("Récompense")]
   
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

    // Update is called once per frame
    void Update()
    {


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
            if (vumark.InstanceId.NumericValue <= quizzLimit)
            {
                currentScriptableQuizz = scriptableQuizzList[vumark.InstanceId.NumericValue - 1];
                if(currentScriptableQuizz.hasBeenDone == true)
                {
                    StartCoroutine(GameWon());
                }
                else
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

                    quizzText.text = currentScriptableQuizz.quizzQuestion;
                    answer1Text = currentScriptableQuizz.answer1;
                    answer2Text = currentScriptableQuizz.answer2;
                    answer3Text = currentScriptableQuizz.answer3;
                    answer4Text = currentScriptableQuizz.answer4;
                    funFactText = currentScriptableQuizz.quizzFunFact;

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
                }
             
            }
            else if (vumark.InstanceId.NumericValue >= quizzLimit && vumark.InstanceId.NumericValue <= miniGameLimit)
            {
                currentScriptableMiniGame = scriptableMiniGameList[(vumark.InstanceId.NumericValue - quizzLimit) - 1];

                if(currentScriptableMiniGame.hasBeenDone == true)
                {
                    StartCoroutine(GameWon());
                }
                else
                {
                    Debug.Log(currentScriptableMiniGame);
                    //scoreToReach = currentScriptableMiniGame.scoreLimit;
                    miniGameToDestroy = currentScriptableMiniGame.prefabMiniJeux;
                    miniGameToDestroy = Instantiate(currentScriptableMiniGame.prefabMiniJeux, miniGameSpawnPoint);
                    //Destroy(GameObject.FindWithTag("ball"));

                }
                                
        
            }

            else if (vumark.InstanceId.NumericValue >= miniGameLimit)
            {
                currentScriptableScan = scriptableScanList[(vumark.InstanceId.NumericValue - miniGameLimit) - 1];
                StartCoroutine(GameWon());
            }
        }

    }
    
    public void OnTrackerLost()
    {
        if(screenShare)
        {
            screenShare.SetActive(false);
        }

        foreach (var item in mVuMarkManager.GetActiveBehaviours())
        {
            int targetObj = System.Convert.ToInt32 (item.VuMarkTarget.InstanceId.NumericValue);
            transform.GetChild(targetObj - 1).gameObject.SetActive(false);
        }
        if(miniGameToDestroy)
        {
            Destroy(miniGameToDestroy);
            Destroy(GameObject.FindWithTag("ball"));
        }
       
        if(quizzInterface)
        {
            quizzInterface.SetActive(false);
        }
        if(congratulationsImage)
        {
            congratulationsImage.SetActive(false);
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

        button1.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
        button2.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
        button3.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);
        button4.GetComponent<UnityEngine.UI.Image>().color = new Color(r, g, b);

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

  
        // Gère le gain de point des mini jeux 

    //public void MiniGameScore(int scoreAdd)
    //{
    //    currentScore = scoreAdd + currentScore;
    //    if(currentScore >= scoreToReach)
    //    {
    //        Destroy(miniGameToDestroy);
    //        Destroy(GameObject.FindWithTag("ball"));
    //        StartCoroutine(GameWon());      
    //    }
    //}

        public void MiniGameScore ()
    {
        Destroy(miniGameToDestroy);
        StartCoroutine(GameWon());   
    }

    IEnumerator GameWon()
    {
        
        yield return new WaitForSeconds(2);

        // Reset du quizz
        screenShare.SetActive(true);
        //currentScore = 0;
        congratulationsImage.SetActive(false);
        quizzInterface.SetActive(false);
        //

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