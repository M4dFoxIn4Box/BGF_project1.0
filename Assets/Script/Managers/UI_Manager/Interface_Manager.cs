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
    public GameObject mainMenuSection;
    public GameObject loadingDoor;
    private GameObject menuToDisplay;
    private GameObject previousDisplayedMenu;

    [Header("Quizz Section")]
    public ScriptableQuizzManager[] quizzManagers;
    public GameObject quizzSection;
    public Transform rightAnswersSection;
    public Transform badAnswersSection;
    public Text quizzQuestionText;
    public Transform quizzAnswersButtonsSection;
    public Color inactiveGoodAnswerFeedbackColor;
    public Color activeGoodAnswerFeedbackColor;
    private List<bool> quizzAnsweredId = new List<bool>();
    private int currentScanId = -1;
    private int currentQuizzRightAnswersNb = 0;
    private int currentQuizzBadAnswersNb = 0;
    private int previousQuestionId = -1;

    [Header("Scan Section")]
    public GameObject ARModeMenu;
    public GameObject feedbackScan;
    public Image feedbackScanImage;
    public Text loadingTextState;
    public float scanDuration;
    private bool isScanning = false;

    [Header("Games Section")]
    public GameObject scoreSection;
    public Text scoreText;
    private int scoreValue = 0;

    [Header("Map")]
    public Transform mapSpots;//Parent map list
    public Color foundSpotColor; //Couleur de l'image sur la map

    [Header("Tutoriel")]
    public GameObject tutoSection;
    public List<AppMessages> tutoMessages;
    private int tutoMsgIdx = 0;
    private bool tutoDone = false;

    //END CODE YANNICK

    private void Awake()
    {
        quizzAnsweredId.Clear();
        for (int i = 0; i < animationsParent.childCount; i++)
        {
            quizzAnsweredId.Add(false);
        }
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
        mainMenuSection.SetActive(true);
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
        if (feedbackScanImage.fillAmount >= 0 && !feedbackScan.activeSelf)
        {
            HideMessage();
            currentScanId = vId;
            feedbackScanImage.fillAmount = 0;
            feedbackScan.SetActive(true);
            isScanning = true;
        }
    }

    public void EndScanning ()
    {
        feedbackScan.SetActive(false);
        feedbackScanImage.fillAmount = 0;
        isScanning = false;
    }

    void Update()
    {
        if (isScanning)
        {
            feedbackScanImage.fillAmount += Time.deltaTime / scanDuration;
            loadingTextState.text = (feedbackScanImage.fillAmount * 100).ToString("F0") + "%";

            if (feedbackScanImage.fillAmount >= 1)
            {
                CheckQuizzState();
            }
        }
    }

    public void CheckQuizzState()
    {
        EndScanning();
        if (!quizzAnsweredId[currentScanId])
        {
            ResetBadAnswersFeedbacks();
            ResetRightAnswersFeedbacks();
            currentQuizzRightAnswersNb = 0;
            currentQuizzBadAnswersNb = 0;
            PopulateQuizzElements();
        }
        else if (quizzAnsweredId[currentScanId])
        {
            if (currentScanId == 14)
            {
                DisplayScore();
            }
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
            quizzAnsweredId[currentScanId] = true;
            DisplayMessage(uiMessages[1]);
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
                fb.GetComponent<Image>().color = inactiveGoodAnswerFeedbackColor;
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
    }

    public void DisplayQuizz()
    {
        quizzSection.SetActive(true);
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

    public void ResetScoreSection ()
    {
        scoreValue = 0;
        scoreText.text = scoreValue.ToString();
    }

    public void OnPickAnswer ()
    {
        Transform cTrs = EventSystem.current.currentSelectedGameObject.transform;
        if (cTrs.GetSiblingIndex() + 1 == quizzManagers[currentScanId - 1].scriptableQuizzList[previousQuestionId].rightAnswer)
        {
            cTrs.GetComponent<Image>().color = Color.green;
            rightAnswersSection.GetChild(currentQuizzRightAnswersNb).GetComponent<Image>().color = activeGoodAnswerFeedbackColor;
            currentQuizzRightAnswersNb++;
            myAnim.SetTrigger("QuizzCorrect");
        }
        else if (cTrs.GetSiblingIndex() + 1 != quizzManagers[currentScanId - 1].scriptableQuizzList[previousQuestionId].rightAnswer)
        {
            cTrs.GetComponent<Image>().color = Color.red;
            badAnswersSection.GetChild(currentQuizzBadAnswersNb).gameObject.SetActive(true);
            currentQuizzBadAnswersNb++;
            myAnim.SetTrigger("QuizzError");
        }
        DisableAnswersButtons();
    }

    public void EnableAnswersButtons()
    {
        foreach (Transform button in quizzAnswersButtonsSection)
        {
            button.GetComponent<Button>().interactable = true;
            button.GetComponent<Image>().color = Color.white;
        }
    }

    public void DisableAnswersButtons ()
    {
        foreach (Transform button in quizzAnswersButtonsSection)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    public void ResetRightAnswersFeedbacks()
    {
        foreach (Transform fb in rightAnswersSection)
        {
            fb.GetComponent<Image>().color = inactiveGoodAnswerFeedbackColor;
        }
    }

    public void ResetBadAnswersFeedbacks()
    {
        foreach (Transform fb in badAnswersSection)
        {
            fb.gameObject.SetActive(false);
        }
    }


    #region Main Gallery
    [Header("Main Gallery")]

    public Transform artifactsGallery;
    private List<int> scanIdx = new List<int>();
    private List<bool> buttonState = new List<bool>();
    private int idxButton;
    #endregion

    #region Score
    [Header("Scoring")]

    //public Text scoreText;//le texte pour afficher le score
    public float score = 0;//le score
    public float limitToWin;//la limite pour finir l'application
    public GameObject victoryText;//texte de victoire pour la limittowin

    private float currentQuestValue;//score à afficher sur la jauge dans le main menu
    public Image questImage;//barre à fillamount
    public List<int> palierScoreList;//index des paliers
    public List<Image> rewardImgList;//lié à l'index des paliers 
    public List<Sprite> rewardSpriteList;//lié à l'index des paliers coffres du menu principal


    //SCORING & PALIER

    //public void AddScore(int newScoreValue)
    //{
    //    Debug.Log("Here");
    //    score = score + newScoreValue;
    //    currentQuestValue = score / limitToWin;
    //    //scoreText.text = "Trésors Découverts \n" + score + " / " + limitToWin;
    //    questImage.fillAmount = currentQuestValue;
    //    //Save_Manager.saving.SavingScore((int)score);
    //    UpdateScore();
    //}

    //void UpdateScore()
    //{
    //    Debug.Log("Here");
    //    if (score == palierScoreList[0])
    //    {
    //        rewardImgList[0].sprite = rewardSpriteList[1];
    //        idxCrateStates[0] = 1;

    //        //Save_Manager.saving.SavingCrateState(idxCrateStates);

    //        //Story_Manager.story.ActivateStoryInGallery(idxStoryScriptableToActivate[0]);
    //    }

    //    if (score == palierScoreList[1])
    //    {
    //        rewardImgList[1].sprite = rewardSpriteList[1];
    //        idxCrateStates[1] = 1;

    //        //Save_Manager.saving.SavingCrateState(idxCrateStates);

    //        //Story_Manager.story.ActivateStoryInGallery(idxStoryScriptableToActivate[1]);
    //    }
    //    if (score == palierScoreList[2])
    //    {
    //        rewardImgList[2].sprite = rewardSpriteList[1];
    //        idxCrateStates[2] = 1;

    //        //Save_Manager.saving.SavingCrateState(idxCrateStates);

    //        //Story_Manager.story.ActivateStoryInGallery(idxStoryScriptableToActivate[2]);
    //    }
    //}
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
        //mainCanvas.worldCamera = arCam;
        //menuToActivate[currentIdxMenu].SetActive(false);
        //ARModeMenu.SetActive(true);
        vumarkSection.SetActive(true);
        uiCam.gameObject.SetActive(false);
        arCam.gameObject.SetActive(true);
        menusBackground.SetActive(false);
        mainMenuSection.SetActive(false);
        ARModeMenu.SetActive(true);
    }

    public void ChangeMenu (GameObject newMenu)
    {
        myAnim.SetTrigger("TransitionDoor");
        if (menuToDisplay != null)
        {
            previousDisplayedMenu = menuToDisplay;
        }
        menuToDisplay = newMenu;
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

    public void CloseARCamera()//ETEINDRE AR CAM
    {
        //if (score >= 1)
        //{
        //    Tuto_Manager.tuto.ActivatingTuto(3);
        //    Story_Manager.story.ActivateStoryInGallery(0);
        //}
        //if (score >= 5)//Quick le joueur pour qu'il puisse découvrir le tuto pour expliquer la récompense
        //{
        //    Tuto_Manager.tuto.ActivatingTuto(4);
        //}
        //mainCanvas.worldCamera = uiCam;
        vumarkSection.SetActive(false);
        uiCam.gameObject.SetActive(true);
        arCam.gameObject.SetActive(false);
        menusBackground.SetActive(true);
        mainMenuSection.SetActive(true);
        ARModeMenu.SetActive(false);
        //ARModeMenu.SetActive(false);
        //menuToActivate[currentIdxMenu].SetActive(true);
    }
    #endregion

    #region Map

    

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

    #region Reward
    [Header("Reward & Box")]

    public List<bool> rewardAlreadyDone;//si le coffre à été récupéré
    public List<int> idxCrateStates;//index du coffre
    private int rewardCounter;

    public void RewardBoxOpened(int rewardBoxIdx)
    {
        Debug.Log("Here");
        rewardImgList[rewardBoxIdx].sprite = rewardSpriteList[2];
        idxCrateStates[rewardBoxIdx] = 2;
        //Save_Manager.saving.SavingCrateState(idxCrateStates);
    }

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

    public void ShowElement(GameObject elementToActive)
    {
        Debug.Log("Here");
        elementToActive.SetActive(true);
    }

    public void UnShowElement(GameObject elementToDesactive)
    {
        Debug.Log("Here");
        elementToDesactive.SetActive(false);
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

    public void SpotFound (int vuMarkIdx)//Maping
    {
        Debug.Log("updated spot");
        mapSpots.GetChild(vuMarkIdx).GetComponent<Image>().color = foundSpotColor;
    }

    #endregion

    #region LOAD Variable
    //LOADING VARIABLE

    //public void CheckStateButton(int idx)//changement de state de la galerie
    //{
    //    Debug.Log("Here");
    //    if (!scanIdx.Contains(idx))
    //    {
    //        scanIdx.Add(idx);
    //        if (artifactsGallery.GetChild(idx).gameObject.GetComponent<Button>().interactable == false)
    //        {
    //            artifactsGallery.GetChild(idx).gameObject.GetComponent<Button>().interactable = true;
    //            //Save_Manager.saving.SetToTrue(idx);
    //            idxButton = idx;
    //            AddScore(1);
    //        }
    //    }
    //}

    //public void ButtonState(List<bool> interactableButton)//load button ok dans la galerie
    //{
    //    Debug.Log("Here");
    //    for (int i = 0; i < interactableButton.Count; i++)
    //    {
    //        artifactsGallery.GetChild(i).gameObject.GetComponent<Button>().interactable = interactableButton[i];
    //        if (interactableButton[i] == true)
    //        {
    //            AddScore(1);
    //        }
    //    }
    //}


    //public void ImageState(List<bool> isImageCheck) //IMAGE ON MAP 
    //{
    //    Debug.Log("Here");
    //    for (int j = 0; j < isImageCheck.Count; j++)
    //    {
    //        if (isImageCheck[j])
    //        {
    //            mapList.GetChild(j).gameObject.GetComponent<Image>().color = mapColor;
    //        }

    //    }
    //}

    public void TutoIsDone(bool isTutoDone)
    {
        Debug.Log("Here");
        quizzDone = isTutoDone;
    }

    //public void LoadScore(int scoring)//load score
    //{
    //    Debug.Log("Here");
    //    score = scoring;
    //    scoreText.text = "Trésors Découverts \n" + score + " / " + limitToWin;
    //    currentQuestValue = score / limitToWin;
    //    questImage.fillAmount = currentQuestValue;
    //}

    //public void LoadCrateImage(List<int> crateVumarkNumber)//load coffre
    //{
    //    Debug.Log("Here");
    //    idxCrateStates = crateVumarkNumber;

    //    for (int j = 0; j < rewardImgList.Count && crateVumarkNumber != null; j++)
    //    {
    //        rewardImgList[j].sprite = rewardSpriteList[crateVumarkNumber[j]];
    //    }
    //}

    #endregion

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
