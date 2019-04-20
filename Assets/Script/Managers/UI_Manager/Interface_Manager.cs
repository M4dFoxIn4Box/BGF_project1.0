﻿using System.Collections;
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
    public int targetIdUnlockingTeaserGame = -1;
    public int targetIdUnlockingMainGame = -1;
    private int currentSpotFound = -1;
    private int spotsCountToUnlockBonus = 0;
    public int spotsCountToUnlockAfterFirstTeaserScan = 4;
    private AudioSource myAS;
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
    public GameObject menuTopUI;
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
    private int currentQuestionId = -1;
    private bool canAnswer = true;
    private List<int> questionsAskedIdx = new List<int>();
    private int questionsCountInQuizz = -1;

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
    public List<AppMessages> scanErrorMessages;

    [Header("Games Section")]
    public GameObject scoreSection;
    public Text scoreText;
    private int scoreValue = 0;

    [Header("Map")]
    public Transform mapSpots; //Parent map spots list

    [Header("Gallery")]
    public List<GameObject> elementsToSpawn;
    public Transform rewardSpawnPoint;
    public Transform buttonsGallery;
    public GameObject funFactUI;
    private GameObject spawnedReward;
    private int spawnedRewardIdx = -1;

    [Header("Badges")]
    public Transform buttonsBadges;
    public GameObject blackBG;
    public Image zoomInBadgeImage;

    [Header("Tutoriel")]
    public List<AppMessages> tutoMessages;
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
        myAS = uiCam.GetComponent<AudioSource>();
        for (int i = 0; i < teaserChallengesSection.childCount; i++)
        {
            if (teaserChallengesSection.GetChild(i).GetComponent<ChallengeState>().UnlockBonus())
            {
                spotsCountToUnlockBonus++;
            }
        }
    }

    public int GetAppTargetsCount ()
    {
        return animationsParent.childCount;
    }

    //Au lancement de l'appli, charge les données précédemment sauvegardées
    public void LoadSaveData ()
    {
        previousDisplayedMenu = null;
        if (!SaveManager.Initialized)
        {
            SaveManager.RetrieveSaveData();
            SetupStartUserEnvironment();
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

    public int GetBadgesNumber ()
    {
        return buttonsBadges.childCount;
    }

    public int GetTargetIdUnlockingMainGame ()
    {
        return targetIdUnlockingMainGame;
    }

    public int GetTargetIdUnlockingTeaserGame()
    {
        return targetIdUnlockingTeaserGame;
    }

    public void SetupStartUserEnvironment ()
    {
        //Si le joueur n'a pas encore fait le tuto, je l'affiche
        if (!SaveManager.Data.firstTutoRead)
        {
            SetupTuto();
            return;
        }

        //Si l'UI au bas de l'appli n'est pas affichée, l'affiche
        if (!bottomUIButtonsParent.gameObject.activeSelf)
        {
            bottomUIButtonsParent.gameObject.SetActive(true);
        }
        
        //Si le joueur a déjà commencé le Main Event...
        if (SaveManager.Data.eventMainStarted)
        {
            //...pour chaque cible...
            for (int i = 0; i < targetIdUnlockingTeaserGame; i++)
            {
                //...si le joueur l'a déjà scannée, je le mets à jour sur la carte
                if (SaveManager.Data.artefactsUnlocked[i])
                {
                    CompleteChallenge(i);
                }
            }
        }
        else if (!SaveManager.Data.eventMainStarted)
        {
            //Si le premier Teaser Spot a déjà été scanné...
            if (SaveManager.Data.artefactsUnlocked[targetIdUnlockingTeaserGame])
            {
                //...je le valide
                CompleteChallenge(targetIdUnlockingTeaserGame);
            }
        }
    }

    //Affiche la porte au début de l'animation (depuis l'Animator)
    public void DisplayLoadingDoor ()
    {
        loadingDoor.SetActive(true);
    }

    //Masque la porte à la fin de l'animation (depuis l'Animator)
    public void HideLoadingDoor()
    {
        loadingDoor.SetActive(false);
    }
    
    //Affiche le premier message du tuto
    public void SetupTuto ()
    {
        DisplayMessage(tutoMessages[0]);
    }

    //Fait dérouler les messages du tuto lorsque le joueur appuie sur "Suivant"
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

    //Affiche un message d'aide depuis le menu "Help"
    public void OnClickHelpButton ()
    {
        Transform cTrs = EventSystem.current.currentSelectedGameObject.transform;
        int cEventIdx = cTrs.GetSiblingIndex();
        DisplayMessage(helpMessages[cEventIdx]);
    }

    //Affiche un message du robot
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

    //Affiche un message du robot
    public void DisplayMessage (string dMsg)
    {
        messageText.text = dMsg;
        messageSection.SetActive(true);
    }

    //Affiche un message d'erreur lorsque le joueur ne scanne pas les cibles dans l'ordre chronologique
    public void DisplayScanErrorMessage(int msgId)
    {
        messageText.text = scanErrorMessages[msgId].messageToDisplay;
        if (scanErrorMessages[msgId].imageToDisplay != null)
        {
            messageImage.sprite = scanErrorMessages[msgId].imageToDisplay;
            messageImage.gameObject.SetActive(true);
        }
        else if (scanErrorMessages[msgId].imageToDisplay == null)
        {
            messageImage.sprite = null;
            messageImage.gameObject.SetActive(false);
        }
        messageSection.SetActive(true);
    }

    //Masque le message du robot
    public void HideMessage()
    {
        messageSection.SetActive(false);
        messageText.text = "";
        messageImage.gameObject.SetActive(false);
        messageImage.sprite = null;
        if (!arCamWithScanEnabled && arCam.gameObject.activeSelf)
        {
            funFactUI.SetActive(true);
        }
    }

    //Comportement de l'UI de l'appli si la cible est perdue de vue par la caméra
    public void LostTracker ()
    {
        HideScore();
        EndScanning();
    }

    //Prépare un affichage UI propre et lance le scan
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

    //Réinitialise l'UI du scan
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
        //Animation de scan lorsque la RA a détecté une cible
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

    //Vérifie si le quizz doit s'afficher ou si le joueur l'a déjà fait, et donc doit afficher l'objet 3D animé
    public void CheckQuizzState()
    {
        EndScanning();
        if (!SaveManager.Data.quizzAnswered[currentScanId])
        {
            ResetQuizzAnswersFeedbacks();
            PopulateQuizzElements();
        }
        else if (SaveManager.Data.quizzAnswered[currentScanId])
        {
            ARModeMenu.SetActive(false);
            RAManager.s_Singleton.DisplayAnimation(currentScanId);
        }
    }

    //Vérifie si le joueur a donné 2 bonnes réponses au quizz, ou 2 mauvaises, ou si le quizz est en cours
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

    //Nouvelle question selon conditions
    public void PopulateQuizzElements ()
    {
        //Si je viens de scanner une nouvelle target, je récupère le nb de questions possibles dans le quizz
        if (questionsCountInQuizz == -1)
        {
            ResetQuizzAnswersFeedbacks();
            questionsCountInQuizz = quizzManagers[currentScanId].scriptableQuizzList.Count;
        }

        //Si toutes les questions du quizz ont déjà été posées, je vide la liste des questions posées
        if (questionsAskedIdx.Count == questionsCountInQuizz)
        {
            questionsAskedIdx.Clear();
        }

        //Je sélectionne une question au hasard parmi le nb de questions possibles dans le quizz
        currentQuestionId = UnityEngine.Random.Range(0, questionsCountInQuizz);

        //Si cette question a déjà été posée, j'en sélectionne une autre
        while (questionsAskedIdx.Contains(currentQuestionId))
        {
            currentQuestionId = UnityEngine.Random.Range(0, questionsCountInQuizz);
        }

        //J'enregistre la nouvelle question comme faisant désormais partie des questions posées
        questionsAskedIdx.Add(currentQuestionId);

        //Je récupère les infos de la question : la question et les 4 réponses possibles
        for (int i = 0; i < quizzManagers[currentScanId].scriptableQuizzList[currentQuestionId].answerList.Length; i++)
        {
            quizzAnswersButtonsSection.GetChild(i).GetComponentInChildren<Text>().text = quizzManagers[currentScanId].scriptableQuizzList[currentQuestionId].answerList[i];
        }
        quizzQuestionText.text = quizzManagers[currentScanId].scriptableQuizzList[currentQuestionId].quizzQuestion;

        //Je rends les boutons 'réponse' interactifs
        EnableAnswersButtons();

        //J'affiche le quizz : bulle, robot, questions et boutons 'réponse'
        DisplayQuizz();

        //Je donne la possibilité au joueur de répondre
        canAnswer = true;
    }

    //Affiche l'UI du quizz
    public void DisplayQuizz()
    {
        if (currentQuizzBadAnswersNb == 0 && currentQuizzRightAnswersNb == 0)
        {
            myAnim.SetTrigger("Idle");
        }
        quizzSection.SetActive(true);
        ARModeMenu.SetActive(false);
    }

    //Masque l'UI du quizz
    public void HideQuizz()
    {
        quizzSection.SetActive(false);
    }

    //Lance le Reset du quizz
    public void ResetQuizz ()
    {
        HideQuizz();
        ResetQuizzAnswersFeedbacks();
        questionsCountInQuizz = -1;
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

    //Vérifie si la réponse donnée par le joueur au quizz est bonne ou pas
    public void OnPickAnswer ()
    {
        if (canAnswer)
        {
            canAnswer = false;
            Transform cTrs = EventSystem.current.currentSelectedGameObject.transform;
            if (cTrs.GetSiblingIndex() + 1 == quizzManagers[currentScanId].scriptableQuizzList[currentQuestionId].rightAnswer)
            {
                cTrs.GetComponent<Image>().sprite = rightAnswerButtonSprite;
                rightAnswersSection.GetChild(currentQuizzRightAnswersNb).GetComponent<Image>().sprite = activeGoodAnswerSprite;
                currentQuizzRightAnswersNb++;
                myAnim.SetTrigger("QuizzCorrect");
            }
            else if (cTrs.GetSiblingIndex() + 1 != quizzManagers[currentScanId].scriptableQuizzList[currentQuestionId].rightAnswer)
            {
                cTrs.GetComponent<Image>().sprite = badAnswerButtonSprite;
                badAnswersSection.GetChild(currentQuizzBadAnswersNb).GetComponent<Image>().sprite = activeBadAnswerSprite;
                currentQuizzBadAnswersNb++;
                myAnim.SetTrigger("QuizzError");
            }
            DisableAnswersButtons(cTrs);
        }
    }

    //Active les boutons Réponse du quizz pour que le joueur puisse répondre
    public void EnableAnswersButtons()
    {
        foreach (Transform button in quizzAnswersButtonsSection)
        {
            button.GetComponent<Button>().interactable = true;
            button.GetComponent<Image>().sprite = badAnswerButtonSprite;
        }
    }

    //Désactive les boutons Réponse du quizz après une réponse donnée par le joueur
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

    //Réinitialise l'UI du quizz
    public void ResetQuizzAnswersFeedbacks ()
    {
        currentQuizzBadAnswersNb = 0;
        currentQuizzRightAnswersNb = 0;
        foreach (Transform fb in badAnswersSection)
        {
            fb.GetComponent<Image>().sprite = inactiveGoodAnswerSprite;
        }
        foreach (Transform fb in rightAnswersSection)
        {
            fb.GetComponent<Image>().sprite = inactiveGoodAnswerSprite;
        }
    }

    //Lance la RA
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
        menuTopUI.SetActive(false);
    }

    //Ferme le menu RA
    public void CloseARCamera()
    {
        if (vumarkSection.activeSelf)
        {
            vumarkSection.SetActive(false);
        }
        uiCam.gameObject.SetActive(true);
        arCam.gameObject.SetActive(false);
        menuBackground.SetActive(true);
        funFactUI.SetActive(false);
        menuTopUI.SetActive(true);
    }

    //Masque le menu en cours et déclenche l'animation de la porte qui se ferme
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

    //Prépare l'affichage de la RA et ferme le menu "Map"
    public void OnClickARButton ()
    {
        arCamWithScanEnabled = true;
        for (int i = 0; i < bottomUIButtonsParent.childCount; i++)
        {
            bottomUIButtonsParent.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }

    //S'exécute quand un bouton du menu au bas de l'appli est cliqué
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

    //Prépare l'affichage de l'artefact en mode Galerie et ferme le menu "Galerie" pour ouvrir la RA
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

    //Permet de zoomer sur un badge en cliquant dessus
    public void OnClickBadgeButton()
    {
        Transform cTrs = EventSystem.current.currentSelectedGameObject.transform;
        zoomInBadgeImage.sprite = cTrs.GetChild(0).GetComponent<Image>().sprite;
        myAnim.SetTrigger("BadgeZoomIn");
    }

    //Permet de dézoomer sur le badge zoomé en cliquant dessus
    public void OnClickBadgeZoomIn()
    {
        myAnim.SetTrigger("BadgeZoomOut");
    }

    //Appelé depuis l'animator lors du zoom/dézoom d'un badge 
    public void ToggleBadgeBlackBG ()
    {
        blackBG.SetActive(!blackBG.activeSelf);
        if (!blackBG.activeSelf)
        {
            myAnim.ResetTrigger("BadgeZoomOut");
            myAnim.ResetTrigger("BadgeZoomIn");
        }
    }

    //Affiche le fun fact
    public void OnClickShowFunFact ()
    {
        funFactUI.SetActive(false);
        DisplayMessage(quizzManagers[spawnedRewardIdx].funFact);
    }

    //Toggle de la musique
    public void OnClickMuteMusicToggle ()
    {
        myAS.mute = !myAS.mute;
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
        //Affiche l'objet 3D animé et le fun fact dans la galerie
        Transform tmpSpawnTrs = rewardSpawnPoint.GetChild(spawnedRewardIdx);
        spawnedReward = Instantiate(elementsToSpawn[spawnedRewardIdx], tmpSpawnTrs.position, tmpSpawnTrs.rotation, tmpSpawnTrs);
        funFactUI.SetActive(true);
    }

    public void DestroySpawnedReward ()
    {
        //Détruit l'objet 3D animé dans la galerie
        if (spawnedReward != null)
        {
            Destroy(spawnedReward);
            spawnedRewardIdx = -1;
        }
    }
    
    public void CompleteChallenge (int spotIdx)
    {
        //Débloque l'artefact et le badge correspondants s'ils ne sont pas déjà débloqués
        UnlockArtefact(spotIdx);
        UnlockBadge(spotIdx);

        //Si c'est le Teaser Event qui est actif, remplit la barre de complétion et affiche les étoiles...
        if (!SaveManager.Data.eventMainStarted)
        {
            teaserChallengesSection.GetChild(spotIdx - targetIdUnlockingTeaserGame).GetComponent<ChallengeState>().SetChallengeCompleted();

            //... et si c'est le premier challenge qui est complété, débloque les éléments suivants
            if (spotIdx == targetIdUnlockingTeaserGame)
            {
                UnlockNextTeaserSpotsToScan();
            }
            //...sinon, si ce n'est pas l'Easter Egg qui est scanné, vérifie s'il doit être débloqué
            else if (spotIdx - targetIdUnlockingTeaserGame != teaserChallengesSection.childCount - 1)
            {
                CheckIfUnlockBonusSpot();
            }
        }
        //...sinon, je mets à jour le spot sur la carte
        else if (SaveManager.Data.eventMainStarted)
        {
            mapSpots.GetChild(spotIdx).GetComponent<MapARSpot>().SetSpotFound();
        }
    }

    public void UnlockNextTeaserSpotsToScan ()
    {
        //Débloque tous les spots que le premier challenge indique
        for (int i = 1; i <= spotsCountToUnlockAfterFirstTeaserScan; i++)
        {
            UnlockNewTeaserSpotToScan(i);
        }
    }

    public void UnlockNewTeaserSpotToScan(int teaserSpotIdx)
    {
        //Affiche le nouveau spot à scanner
        teaserChallengesSection.GetChild(teaserSpotIdx).gameObject.SetActive(true);
        int realIdx = teaserSpotIdx + targetIdUnlockingTeaserGame;
        if (SaveManager.Data.artefactsUnlocked[realIdx])
        {
            CompleteChallenge(realIdx);
        }
    }

    //Vérifie si le Bonus Spot doit être débloqué
    public void CheckIfUnlockBonusSpot ()
    {
        int bonusSpotsCountCheck = 0;
        for (int i = 0; i < teaserChallengesSection.childCount; i++)
        {
            ChallengeState tmpCS = teaserChallengesSection.GetChild(i).GetComponent<ChallengeState>();
            if (tmpCS.IsChallengeCompleted() && tmpCS.UnlockBonus())
            {
                bonusSpotsCountCheck++;
            }
        }
        if (bonusSpotsCountCheck == spotsCountToUnlockBonus)
        {
            UnlockNewTeaserSpotToScan(teaserChallengesSection.childCount - 1);
        }
    }

    //Débloque un artefact dans le menu "Galerie"
    public void UnlockArtefact (int afIdx)
    {
        Button tmpBtn = buttonsGallery.GetChild(afIdx).GetComponent<Button>();
        if (!tmpBtn.interactable)
        {
            tmpBtn.interactable = true;
        }
    }

    //Débloque les badges associés aux artefacts, et les badges bonus
    public void UnlockBadge (int badgeIdx)
    {
        int badgesCount = SaveManager.Data.badgesUnlocked.Count;
        Button tmpBtn = buttonsBadges.GetChild(badgeIdx).GetComponent<Button>();
        if (!tmpBtn.interactable)
        {
            buttonsBadges.GetChild(badgeIdx).GetComponent<Button>().interactable = true;
        }

        //Si l'Easter Egg vient d'être scanné, débloque le badge de récompense du Teaser Event
        if (badgeIdx == SaveManager.Data.artefactsUnlocked.Count -1)
        {
            SaveManager.Data.badgesUnlocked[badgesCount - 2] = true;
            buttonsBadges.GetChild(badgesCount - 2).GetComponent<Button>().interactable = true;
        }

        //Si le dernier badge n'est pas débloqué...
        if (!SaveManager.Data.badgesUnlocked[badgesCount - 1])
        {
            //...je compte combien d'artefacts des deux events ont été débloqués.
            int afUnlocked = 0;
            for (int i = 0; i < SaveManager.Data.artefactsUnlocked.Count; i++)
            {
                if (SaveManager.Data.artefactsUnlocked[i])
                {
                    afUnlocked++;
                }
            }
            //Si le joueur a débloqué TOUS les artefacts des deux events, je débloque le dernier badge
            if (afUnlocked == SaveManager.Data.artefactsUnlocked.Count)
            {
                SaveManager.Data.badgesUnlocked[badgesCount - 1] = true;
                buttonsBadges.GetChild(badgesCount - 1).GetComponent<Button>().interactable = true;
            }
        }
    }
    
    public void QuitAPK()
    {
        Application.Quit();
    }
}
