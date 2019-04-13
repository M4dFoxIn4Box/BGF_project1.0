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

    [Header("Events")]
    public int vumarkIdUnlockingTeaserGame = -1;
    public int vumarkIdUnlockingMainGame = -1;
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
    public GameObject teaserEventSection;
    public Transform teaserChallengesSection;
    public GameObject mainEventSection;
    public GameObject menuGallerySection;
    public GameObject menuBadgesSection;
    public GameObject loadingDoor;
    public GameObject aRSection;
    public GameObject menuBackground;
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
    private bool arCamWithScanEnabled = true;
    public Camera arCam;
    public Camera uiCam;
    public GameObject vumarkSection;

    [Header("Games Section")]
    public GameObject scoreSection;
    public Text scoreText;
    private int scoreValue = 0;

    [Header("Map")]
    public Transform mapSpots;//Parent map list

    [Header("Gallery")]
    public List<GameObject> elementsToSpawn;
    public Transform rewardSpawnPoint;
    public Transform buttonsGallery;
    private GameObject spawnedReward;
    private int spawnedRewardIdx = -1;

    [Header("Tutoriel")]
    public List<AppMessages> tutoMessages;
    public GameObject helpSection;
    public List<AppMessages> helpMessages;
    private int tutoMsgIdx = 0;
    
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
    }

    public int GetAppVuMarksCount ()
    {
        return animationsParent.childCount;
    }

    public void LoadSaveData ()
    {
        previousDisplayedMenu = null;
        if (!SaveManager.Initialized)
        {
            SaveManager.RetrieveSaveData();
            UpdateUserEnvironment();
        }

        if (SaveManager.Data.eventMainStarted)
        {
            previousDisplayedMenu = teaserEventSection;
            menuToDisplay = mainEventSection;
        }
        else if ((SaveManager.Data.eventTeaserStarted && !SaveManager.Data.eventMainStarted) || SaveManager.Data.firstTutoRead)
        {
            previousDisplayedMenu = null;
            menuToDisplay = teaserEventSection;
        }
    }

    public int GetVuMarkUnlockingMainGame ()
    {
        return vumarkIdUnlockingMainGame;
    }

    public int GetVuMarkUnlockingTeaserGame()
    {
        return vumarkIdUnlockingTeaserGame;
    }

    public void UpdateUserEnvironment ()
    {
        if (!SaveManager.Data.firstTutoRead)
        {
            Debug.Log("Tuto oui");
            SetupTuto();
            return;
        }
        bottomUIButtonsParent.gameObject.SetActive(true);
        if (SaveManager.Data.artefactsUnlocked[vumarkIdUnlockingTeaserGame - 1])
        {
            for (int j = vumarkIdUnlockingTeaserGame - 1; j < SaveManager.Data.artefactsUnlocked.Count - 1; j++)
            {
                TeaserSpotUnlocked(j);
            }
        }
        if (SaveManager.Data.eventMainStarted)
        {
            int tmpArtfNb = SaveManager.Data.artefactsUnlocked.Count;
            for (int i = 0; i < vumarkIdUnlockingTeaserGame - 1; i++)
            {
                if (SaveManager.Data.artefactsUnlocked[i])
                {
                    SpotFound(i);
                }
            }
        }
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
        DisplayMessage(tutoMessages[0]);
    }

    public void OnClickNextMessage ()
    {
        if (!SaveManager.Data.firstTutoRead)
        {
            tutoMsgIdx++;
            if (tutoMsgIdx == tutoMessages.Count)
            {
                if (!bottomUIButtonsParent.gameObject.activeSelf)
                {
                    bottomUIButtonsParent.gameObject.SetActive(true);
                }
                ChangeMenu(teaserEventSection);
                SaveManager.Data.firstTutoRead = true;
                SaveManager.SaveToFile();
                return;
            }
            DisplayMessage(tutoMessages[tutoMsgIdx]);
            return;
        }
        HideMessage();
    }

    public void OnClickHelpButton ()
    {
        Transform cTrs = EventSystem.current.currentSelectedGameObject.transform;
        int cEventIdx = cTrs.GetSiblingIndex();
        DisplayMessage(helpMessages[cEventIdx]);
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

    public void DisplayMessage (string dMsg)
    {
        messageText.text = dMsg;
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
        HideScore();
        EndScanning();
    }

    public void StartScanning (int vId)
    {
        if (feedbackScanImage.fillAmount >= 0)
        {
            HideMessage();
            currentScanId = vId - 1;
            feedbackScanImage.fillAmount = 0;
            isScanning = true;
        }
    }

    public void EndScanning ()
    {
        feedbackScanImage.fillAmount = 0;
        loadingArrowParent.transform.rotation = Quaternion.identity;
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
        ResetQuizz();
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
        
        int rndInt = UnityEngine.Random.Range(0, quizzManagers[currentScanId].scriptableQuizzList.Count);
        while (rndInt == previousQuestionId)
        {
            rndInt = UnityEngine.Random.Range(0, quizzManagers[currentScanId].scriptableQuizzList.Count);
        }
        for (int i = 0; i < quizzManagers[currentScanId].scriptableQuizzList[rndInt].answerList.Length; i++)
        {
            quizzAnswersButtonsSection.GetChild(i).GetComponentInChildren<Text>().text = quizzManagers[currentScanId].scriptableQuizzList[rndInt].answerList[i];
        }
        quizzQuestionText.text = quizzManagers[currentScanId].scriptableQuizzList[rndInt].quizzQuestion;
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

    public void ResetQuizz ()
    {
        HideQuizz();
        ResetBadAnswersFeedbacks();
        ResetRightAnswersFeedbacks();
        currentQuizzBadAnswersNb = 0;
        currentQuizzRightAnswersNb = 0;
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
            if (cTrs.GetSiblingIndex() + 1 == quizzManagers[currentScanId].scriptableQuizzList[previousQuestionId].rightAnswer)
            {
                cTrs.GetComponent<Image>().sprite = rightAnswerButtonSprite;
                rightAnswersSection.GetChild(currentQuizzRightAnswersNb).GetComponent<Image>().sprite = activeGoodAnswerSprite;
                currentQuizzRightAnswersNb++;
                myAnim.SetTrigger("QuizzCorrect");
            }
            else if (cTrs.GetSiblingIndex() + 1 != quizzManagers[currentScanId].scriptableQuizzList[previousQuestionId].rightAnswer)
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
        
    public void OpenARCamera()
    {
        if (arCamWithScanEnabled)
        {
            vumarkSection.SetActive(true);
            myAnim.SetTrigger("ARMode");
        }
        uiCam.gameObject.SetActive(false);
        arCam.gameObject.SetActive(true);
        menuBackground.SetActive(false);
    }

    public void CloseARCamera()
    {
        if (vumarkSection.activeSelf)
        {
            vumarkSection.SetActive(false);
        }
        uiCam.gameObject.SetActive(true);
        arCam.gameObject.SetActive(false);
        menuBackground.SetActive(true);
    }

    public void ChangeMenu (GameObject newMenu)
    {
        DestroySpawnedReward();
        myAnim.SetTrigger("TransitionDoor");
        if (menuToDisplay != null)
        {
            previousDisplayedMenu = menuToDisplay;
            if (newMenu == teaserEventSection && SaveManager.Data.eventMainStarted)
            {
                menuToDisplay = mainEventSection;
                return;
            }
        }
        menuToDisplay = newMenu;
    }

    public void OnClickARButton ()
    {
        arCamWithScanEnabled = true;
        for (int i = 0; i < bottomUIButtonsParent.childCount; i++)
        {
            bottomUIButtonsParent.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }

    public void OnClickBottomUIButton ()
    {
        ResetQuizz();
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

    public void OnClickArtefactButton ()
    {
        Transform cTrs = EventSystem.current.currentSelectedGameObject.transform;
        spawnedRewardIdx = cTrs.GetSiblingIndex();
        arCamWithScanEnabled = false;
        for (int i = 0; i < bottomUIButtonsParent.childCount; i++)
        {
            bottomUIButtonsParent.GetChild(i).GetComponent<Button>().interactable = true;
        }
        ChangeMenu(ARModeMenu);
    }

    public void DisplayNewMenu ()
    {
        if (SaveManager.Data.firstTutoRead)
        {
            HideMessage();
        }
        if (previousDisplayedMenu != null)
        {
            if (previousDisplayedMenu == teaserEventSection || previousDisplayedMenu == mainEventSection)
            {
                aRSection.SetActive(false);
            }
            previousDisplayedMenu.SetActive(false);
        }

        if (menuToDisplay != null)
        {
            if (menuToDisplay == teaserEventSection || menuToDisplay == mainEventSection)
            {
                aRSection.SetActive(true);
                if (menuToDisplay == teaserEventSection && !SaveManager.Data.teaserEventTutoRead)
                {
                    DisplayMessage(uiMessages[4]);
                    SaveManager.Data.teaserEventTutoRead = true;
                    SaveManager.SaveToFile();
                }
                else if (menuToDisplay == mainEventSection && !SaveManager.Data.mainEventTutoRead)
                {
                    DisplayMessage(uiMessages[5]);
                    SaveManager.Data.mainEventTutoRead = true;
                    SaveManager.SaveToFile();
                }
            }

            if (menuToDisplay == menuGallerySection && !SaveManager.Data.galleryTutoRead)
            {
                DisplayMessage(uiMessages[2]);
                SaveManager.Data.galleryTutoRead = true;
                SaveManager.SaveToFile();
            }
            else if (menuToDisplay == menuBadgesSection && !SaveManager.Data.badgeTutoRead)
            {
                DisplayMessage(uiMessages[3]);
                SaveManager.Data.badgeTutoRead = true;
                SaveManager.SaveToFile();
            }

            menuToDisplay.SetActive(true);
        }
        
        if (menuToDisplay == ARModeMenu)
        {
            if (!arCamWithScanEnabled)
            {
                menuToDisplay.SetActive(false);
                SpawnGalleryRewardWithFunFact();
            }
            OpenARCamera();
        }
        else if (previousDisplayedMenu == ARModeMenu)
        {
            CloseARCamera();
        }
    }

    public void SpawnGalleryRewardWithFunFact ()
    {
        Transform tmpSpawnTrs = rewardSpawnPoint.GetChild(spawnedRewardIdx);
        spawnedReward = Instantiate(elementsToSpawn[spawnedRewardIdx], tmpSpawnTrs.position, tmpSpawnTrs.rotation, tmpSpawnTrs);
        DisplayMessage(quizzManagers[spawnedRewardIdx].funFact);
    }

    public void DestroySpawnedReward ()
    {
        if (spawnedReward != null)
        {
            Destroy(spawnedReward);
            spawnedRewardIdx = -1;
        }
    }

    public void TeaserSpotUnlocked (int newUnlockedSpot)
    {
        teaserChallengesSection.GetChild(newUnlockedSpot - vumarkIdUnlockingTeaserGame + 1).gameObject.SetActive(true);
        if (SaveManager.Data.artefactsUnlocked[newUnlockedSpot])
        {
            TeaserSpotFound(newUnlockedSpot);
        }
    }
    public void TeaserSpotFound (int spotIdx)
    {
        teaserChallengesSection.GetChild(spotIdx - vumarkIdUnlockingTeaserGame + 1).GetComponent<ChallengeState>().SetChallengeCompleted();
        buttonsGallery.GetChild(spotIdx).GetComponent<Button>().interactable = true;
        for (int i = 0; i < teaserChallengesSection.childCount - 1; i++)
        {
            if (!teaserChallengesSection.GetChild(i).GetComponent<ChallengeState>().IsChallengeCompleted())
            {
                return;
            }
        }
        TeaserSpotUnlocked(SaveManager.Data.artefactsUnlocked.Count - 1);
    }

    public void SpotFound(int vuMarkIdx)
    {
        if (!SaveManager.Data.eventMainStarted)
        {
            TeaserSpotFound(vuMarkIdx);
        }
        else if (SaveManager.Data.eventMainStarted)
        {
            mapSpots.GetChild(vuMarkIdx).GetComponent<MapARSpot>().SetSpotFound();
        }
        buttonsGallery.GetChild(vuMarkIdx).GetComponent<Button>().interactable = true;
    }

    public void QuitAPK()
    {
        Application.Quit();
    }
}
