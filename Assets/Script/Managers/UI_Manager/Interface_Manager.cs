using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class Interface_Manager : MonoBehaviour
{
    public static Interface_Manager Instance { get; private set; }

    //CODE YANNICK

    public Transform animationsParent;
    private Animator myAnim;

    [Header("Message Section")]
    public GameObject messageSection;
    public Text messageText;
    public Image messageImage;
    public List<AppMessages> uiMessages;

    [Serializable]
    public class AppMessages
    {
        public string messageName;
        public string messageToDisplay;
        public Sprite imageToDisplay;
    }

    [Header("Main Menu")]
    public GameObject mapSection;
    public GameObject loadingDoor;
    private GameObject menuToDisplay;
    private GameObject previousDisplayedMenu;
    public Transform bottomUIButtonsParent;

    [Header("Quizz Section")]
    public ScriptableQuizzManager[] quizzManagers;
    public GameObject quizzSection;
    public Transform rightAnswersSection;
    public Transform badAnswersSection;
    public Text quizzQuestionText;
    public Transform quizzAnswersButtonsSection;
    public Sprite rightAnswerButtonSprite;
    public Sprite badAnswerButtonSprite;
    public Sprite inactiveGoodAnswerSprite;
    public Sprite activeGoodAnswerSprite;
    public Sprite activeBadAnswerSprite;
    public Image correctAnswersBackground;
    public Image badAnswersBackground;
    private int currentScanId = -1;
    private int currentQuizzRightAnswersNb = 0;
    private int currentQuizzBadAnswersNb = 0;
    private int previousQuestionId = -1;
    private bool canAnswer = true;

    [Header("Scan Section")]
    public GameObject ARModeMenu;
    public Image feedbackScanImage;
    public Transform loadingArrowParent;
    public float scanDuration;
    private bool isScanning = false;

    [Header("Games Section")]
    public GameObject scoreSection;
    public Text scoreText;
    private int scoreValue = 0;

    [Header("Map")]
    public Transform mapSpots;//Parent map list

    [Header("Tutoriel")]
    public GameObject tutoSection;
    public List<AppMessages> tutoMessages;
    private int tutoMsgIdx = 0;
    private bool tutoDone = false;
    
    //END CODE YANNICK

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

    void Start()
    {
        myAnim = GetComponent<Animator>();
        previousDisplayedMenu = mapSection;
        //if (!tutoDone)
        //{
        //    SetupTuto();
        //}
        //scoreText.text = "Trésors Découverts \n" + score + " / " + limitToWin;
    }

    public void DisplayLoadingDoor ()
    {
        loadingDoor.SetActive(true);
    }

    public void HideLoadingDoor()
    {
        loadingDoor.SetActive(false);
    }

    public void SetupTuto ()
    {
        tutoSection.SetActive(true);
        DisplayMessage(tutoMessages[0]);
    }

    public void SetupMainMenu ()
    {
        if (tutoDone)
        {
            if (tutoSection.activeSelf)
            {
                tutoSection.SetActive(false);
            }
        }
        mapSection.SetActive(true);
    }

    public void OnClickNextTutoMessage ()
    {
        tutoMsgIdx++;
        if (tutoMsgIdx == tutoMessages.Count)
        {
            tutoDone = true;
            SetupMainMenu();
            return;
        }
        DisplayMessage(tutoMessages[tutoMsgIdx]);
    }

    public void DisplayMessage (AppMessages dMsg)
    {
        messageText.text = dMsg.messageToDisplay;
        if (dMsg.imageToDisplay != null)
        {
            messageImage.sprite = dMsg.imageToDisplay;
            messageImage.gameObject.SetActive(true);
        }
        else if (dMsg.imageToDisplay == null)
        {
            messageImage.sprite = null;
            messageImage.gameObject.SetActive(false);
        }
        messageSection.SetActive(true);
    }

    public void HideMessage()
    {
        messageSection.SetActive(false);
        messageText.text = "";
        messageImage.gameObject.SetActive(false);
        messageImage.sprite = null;
    }

    public void LostTracker ()
    {
        HideMessage();
        HideScore();
        EndScanning();
    }

    public void StartScanning (int vId)
    {
        if (feedbackScanImage.fillAmount >= 0)
        {
            HideMessage();
            currentScanId = vId;
            feedbackScanImage.fillAmount = 0;
            isScanning = true;
        }
    }

    public void EndScanning ()
    {
        feedbackScanImage.fillAmount = 0;
        isScanning = false;
        if (!quizzSection.activeSelf)
        {
            ARModeMenu.SetActive(true);
        }
    }

    void Update()
    {
        if (isScanning)
        {
            feedbackScanImage.fillAmount += Time.deltaTime / scanDuration;
            loadingArrowParent.Rotate(-transform.forward, 360/scanDuration * Time.deltaTime);

            if (feedbackScanImage.fillAmount >= 1)
            {
                CheckQuizzState();
            }
        }
    }

    public void CheckQuizzState()
    {
        EndScanning();
        if (!SaveManager.Data.quizzAnswered[currentScanId])
        {
            ResetBadAnswersFeedbacks();
            ResetRightAnswersFeedbacks();
            PopulateQuizzElements();
        }
        else if (SaveManager.Data.quizzAnswered[currentScanId])
        {
            ARModeMenu.SetActive(false);
            ScriptTracker.Instance.ActiveAnimation();
        }
    }

    public void CheckAnswersStatus ()
    {
        if (currentQuizzBadAnswersNb >= 2)
        {
            DisplayMessage(uiMessages[0]);
        }
        else if (currentQuizzRightAnswersNb >= 2)
        {
            SaveManager.Data.quizzAnswered[currentScanId] = true;
            SaveManager.SaveToFile();
            DisplayMessage(uiMessages[1]);
            ARModeMenu.SetActive(true);
            myAnim.SetTrigger("ARMode");
        }
        else
        {
            PopulateQuizzElements();
            return;
        }
        HideQuizz();
        ResetBadAnswersFeedbacks();
        ResetRightAnswersFeedbacks();
        currentQuizzBadAnswersNb = 0;
        currentQuizzRightAnswersNb = 0;
    }

    public void PopulateQuizzElements ()
    {
        if (currentQuizzRightAnswersNb == 0 && currentQuizzBadAnswersNb == 0)
        {
            foreach (Transform fb in rightAnswersSection)
            {
                fb.GetComponent<Image>().sprite = inactiveGoodAnswerSprite;
            }
        }
        
        int rndInt = UnityEngine.Random.Range(0, quizzManagers[currentScanId-1].scriptableQuizzList.Count);
        while (rndInt == previousQuestionId)
        {
            rndInt = UnityEngine.Random.Range(0, quizzManagers[currentScanId - 1].scriptableQuizzList.Count);
        }
        for (int i = 0; i < quizzManagers[currentScanId - 1].scriptableQuizzList[rndInt].answerList.Length; i++)
        {
            quizzAnswersButtonsSection.GetChild(i).GetComponentInChildren<Text>().text = quizzManagers[currentScanId - 1].scriptableQuizzList[rndInt].answerList[i];
        }
        quizzQuestionText.text = quizzManagers[currentScanId - 1].scriptableQuizzList[rndInt].quizzQuestion;
        EnableAnswersButtons();
        DisplayQuizz();
        previousQuestionId = rndInt;
        canAnswer = true;
    }

    public void DisplayQuizz()
    {
        if (currentQuizzBadAnswersNb == 0 && currentQuizzRightAnswersNb == 0)
        {
            myAnim.SetTrigger("Idle");
        }
        quizzSection.SetActive(true);
        ARModeMenu.SetActive(false);
    }

    public void HideQuizz()
    {
        quizzSection.SetActive(false);
    }

    public void DisplayScore ()
    {
        scoreSection.SetActive(true);
        ResetScoreSection();
    }

    public void HideScore()
    {
        scoreSection.SetActive(false);
    }

    public void AddScore()
    {
        scoreValue ++;
        scoreText.text = scoreValue.ToString();
    }

    public void AddScore(int sValue)
    {
        scoreValue += sValue;
        scoreText.text = scoreValue.ToString();
    }

    public int GetCurrentScore ()
    {
        return scoreValue;
    }

    public void ResetScoreSection ()
    {
        scoreValue = 0;
        scoreText.text = scoreValue.ToString();
    }

    public void OnPickAnswer ()
    {
        if (canAnswer)
        {
            canAnswer = false;
            Transform cTrs = EventSystem.current.currentSelectedGameObject.transform;
            if (cTrs.GetSiblingIndex() + 1 == quizzManagers[currentScanId - 1].scriptableQuizzList[previousQuestionId].rightAnswer)
            {
                cTrs.GetComponent<Image>().sprite = rightAnswerButtonSprite;
                rightAnswersSection.GetChild(currentQuizzRightAnswersNb).GetComponent<Image>().sprite = activeGoodAnswerSprite;
                currentQuizzRightAnswersNb++;
                myAnim.SetTrigger("QuizzCorrect");
            }
            else if (cTrs.GetSiblingIndex() + 1 != quizzManagers[currentScanId - 1].scriptableQuizzList[previousQuestionId].rightAnswer)
            {
                cTrs.GetComponent<Image>().sprite = badAnswerButtonSprite;
                badAnswersSection.GetChild(currentQuizzBadAnswersNb).GetComponent<Image>().sprite = activeBadAnswerSprite;
                currentQuizzBadAnswersNb++;
                myAnim.SetTrigger("QuizzError");
            }
            DisableAnswersButtons(cTrs);
        }
    }

    public void EnableAnswersButtons()
    {
        foreach (Transform button in quizzAnswersButtonsSection)
        {
            button.GetComponent<Button>().interactable = true;
            button.GetComponent<Image>().sprite = badAnswerButtonSprite;
        }
    }

    public void DisableAnswersButtons (Transform exceptButton)
    {
        foreach (Transform button in quizzAnswersButtonsSection)
        {
            if (button != exceptButton)
            {
                button.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void ResetRightAnswersFeedbacks()
    {
        currentQuizzRightAnswersNb = 0;
        foreach (Transform fb in rightAnswersSection)
        {
            fb.GetComponent<Image>().sprite = inactiveGoodAnswerSprite;
        }
    }

    public void ResetBadAnswersFeedbacks()
    {
        currentQuizzBadAnswersNb = 0;
        foreach (Transform fb in badAnswersSection)
        {
            fb.GetComponent<Image>().sprite = inactiveGoodAnswerSprite;
        }
    }


    #region Main Gallery
    [Header("Main Gallery")]

    public Transform artifactsGallery;
    private List<int> scanIdx = new List<int>();
    private List<bool> buttonState = new List<bool>();
    private int idxButton;
    #endregion

    

    #region Camera

    [Header("Camera / AR Camera")]

    public Camera arCam;//AR Camera
    public Camera uiCam;//UI Camera
    public Canvas mainCanvas;//Main canvas
    public Button buttonARMode;//Buton AR Mode
    public GameObject vumarkSection;//Vumark to activate/deactivate

    public void OpenARCamera()//ALLUMER L AR CAM
    {
        vumarkSection.SetActive(true);
        uiCam.gameObject.SetActive(false);
        arCam.gameObject.SetActive(true);
        menusBackground.SetActive(false);
        myAnim.SetTrigger("ARMode");
        SaveManager.RetrieveSaveData();
    }

    public void CloseARCamera()//ETEINDRE AR CAM
    {
        vumarkSection.SetActive(false);
        uiCam.gameObject.SetActive(true);
        arCam.gameObject.SetActive(false);
        menusBackground.SetActive(true);
        //myAnim.SetTrigger("Idle");
    }

    public void ChangeMenu (GameObject newMenu)
    {
        HideMessage();
        myAnim.SetTrigger("TransitionDoor");
        if (menuToDisplay != null)
        {
            previousDisplayedMenu = menuToDisplay;
        }
        menuToDisplay = newMenu;
    }

    public void OnClickBottomUIButton ()
    {
        Transform cTrs = EventSystem.current.currentSelectedGameObject.transform;
        cTrs.GetComponent<Button>().interactable = false;
        for (int i = 0; i < bottomUIButtonsParent.childCount; i++)
        {
            if (bottomUIButtonsParent.GetChild(i) != cTrs)
            {
                bottomUIButtonsParent.GetChild(i).GetComponent<Button>().interactable = true;
            }
        }
    }

    public void DisplayNewMenu ()
    {
        menuToDisplay.SetActive(true);
        if (previousDisplayedMenu != null)
        {
            previousDisplayedMenu.SetActive(false);
        }
        
        if (menuToDisplay == ARModeMenu)
        {
            OpenARCamera();
        }
        else if (previousDisplayedMenu == ARModeMenu)
        {
            CloseARCamera();
        }
    }
    #endregion

    #region Menu

    [Header("Menu")]//Changer de menu
    private int currentIdxMenu = 0;//Idx du menu intro
    public GameObject[] menuToActivate;//menu à activer
   
    #endregion

    #region Quizz
    [Header("Tutoriel quizz")]

    public int tutoQuizzIdx;//l'index du quizz
    public bool quizzDone;//bool si le tuto à été fait et qui envoyé au save manager

    #endregion
    
    #region Story
    [Header("Story")]

    public List<int> idxStoryScriptableToActivate;//index à envoyé pour activer la bonne histoire
    private bool storyToActivate = false;

    #endregion

    #region Sounds
    [Header("Sounds")]

    public AudioClip audioChangeMenu;
    public AudioMixerGroup[] mixerGroupChangeMenu;

    public AudioSource musicMainMenuToDeactivate;
    private bool stopMusicInGallery = false;

    #endregion

    #region Interface
    [Header("Interface")]

    public GameObject menusBackground;
    private int currentScreenIdx = 0;
    private int previousScreenIdx = -1;

    //Pour changer de menu il faut renseigner le int sur le bouton
    public void ChangeMenu(int newScreenIdx)
    {
        AudioManager.s_Singleton.PlaySFX(audioChangeMenu);
        AudioManager.s_Singleton.GetComponent<AudioSource>().outputAudioMixerGroup = mixerGroupChangeMenu[0];
        previousScreenIdx = currentScreenIdx;
        menuToActivate[currentIdxMenu].SetActive(false);
        currentScreenIdx = newScreenIdx;
        menuToActivate[newScreenIdx].SetActive(true);

        if (stopMusicInGallery)
        {
            DeactiveMusicMainMenu();
        }

        switch (newScreenIdx)
        {
            case 4:
                OpenARCamera();
                break;
        }
    }

    public void BackToPreviousScreen()
    {
        switch (currentScreenIdx)
        {
            case 4:
                CloseARCamera();
                break;
        }
        menuToActivate[currentScreenIdx].SetActive(false);
        currentScreenIdx = previousScreenIdx;
        previousScreenIdx = -1;
        menuToActivate[currentScreenIdx].SetActive(true);
    }

    public void DeactiveMusicMainMenu()//Musique à désactiver ou activer dans la galerie
    {
        Debug.Log("Here");
        if (stopMusicInGallery)
        {
            musicMainMenuToDeactivate.UnPause();
            stopMusicInGallery = false;
        }
        else if (!stopMusicInGallery)
        {
            musicMainMenuToDeactivate.Pause();
            stopMusicInGallery = true;
        }
    }

    //MAP MENU UPDATE

    public void SpotFound(int vuMarkIdx)//Maping
    {
        Debug.Log("updated spot");
        mapSpots.GetChild(vuMarkIdx).GetComponent<MapARSpot>().SetSpotFound();
    }

    #endregion

    public void TutoIsDone(bool isTutoDone)
    {
        Debug.Log("Here");
        quizzDone = isTutoDone;
    }
    
    #region Funfact

    [Header("FunFact")]

    public GameObject funfact;
    private bool isFunFactActive;
    public Image imageFunfactState;
    public Sprite[] spriteFunfactState;

    public void FunFactToggle()
    {
        Debug.Log("Here");
        if (funfact.activeSelf == true)
        {
            funfact.SetActive(false);
            imageFunfactState.sprite = spriteFunfactState[0];
        }
        else if (funfact.activeSelf == false)
        {
            funfact.SetActive(true);
            imageFunfactState.sprite = spriteFunfactState[1];
        }
    }
    #endregion
    
    public void QuitAPK()
    {
        Application.Quit();
    } 

    public void BackToHomeMenu()
    {
        StartCoroutine(LoadHomeScene());
    }

    IEnumerator LoadHomeScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene_Home");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
