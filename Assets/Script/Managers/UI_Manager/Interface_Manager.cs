using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class Interface_Manager : MonoBehaviour
{
    public static Interface_Manager Instance { get; private set; }

    #region Main Gallery
    [Header("Main Gallery")]

    public Transform artifactsGallery;
    private List<int> scanIdx = new List<int>();
    private List<bool> buttonState = new List<bool>();
    private int idxButton;
    #endregion

    #region Score
    [Header("Scoring")]

    public Text scoreText;//le texte pour afficher le score
    public float score;//le score
    public float limitToWin;//la limite pour finir l'application
    public GameObject victoryText;//texte de victoire pour la limittowin

    private float currentQuestValue;//score à afficher sur la jauge dans le main menu
    public Image questImage;//barre à fillamount
    public List<int> palierScoreList;//index des paliers
    public List<Image> rewardImgList;//lié à l'index des paliers 
    public List<Sprite> rewardSpriteList;//lié à l'index des paliers coffres du menu principal


    //SCORING & PALIER

    public void AddScore(int newScoreValue)
    {
        Debug.Log("Here");
        score = score + newScoreValue;
        currentQuestValue = score / limitToWin;
        scoreText.text = "Trésors Découverts \n" + score + " / " + limitToWin;
        questImage.fillAmount = currentQuestValue;
        Save_Manager.saving.SavingScore((int)score);
        UpdateScore();
    }


    void UpdateScore()
    {
        Debug.Log("Here");
        if (score == palierScoreList[0])
        {
            rewardImgList[0].sprite = rewardSpriteList[1];
            idxCrateStates[0] = 1;

            Save_Manager.saving.SavingCrateState(idxCrateStates);

            Story_Manager.story.ActivateStoryInGallery(idxStoryScriptableToActivate[0]);
        }

        if (score == palierScoreList[1])
        {
            rewardImgList[1].sprite = rewardSpriteList[1];
            idxCrateStates[1] = 1;

            Save_Manager.saving.SavingCrateState(idxCrateStates);

            Story_Manager.story.ActivateStoryInGallery(idxStoryScriptableToActivate[1]);
        }
        if (score == palierScoreList[2])
        {
            rewardImgList[2].sprite = rewardSpriteList[1];
            idxCrateStates[2] = 1;

            Save_Manager.saving.SavingCrateState(idxCrateStates);

            Story_Manager.story.ActivateStoryInGallery(idxStoryScriptableToActivate[2]);
        }
    }
    #endregion

    #region Camera

    [Header("Camera / AR Camera")]

    public Camera arCam;//AR Camera
    public Camera uiCam;//UI Camera
    public Canvas mainCanvas;//Main canvas
    public Button buttonARMode;//Buton AR Mode
    public GameObject vumarkPrefab;//Vumark to activate/deactivate

    public void OpenARCamera()//ALLUMER L AR CAM
    {
        Debug.Log("Here");
        if (quizzDone == false)
        {
            Tuto_Manager.tuto.ActivatingTuto(tutoQuizzIdx);
            quizzDone = true;
            Save_Manager.saving.TutoQuizzIsDone(quizzDone);
        }
        else
        {
            mainCanvas.worldCamera = arCam;
            menuToActivate[currentIdxMenu].SetActive(false);
            ARModeMenu.SetActive(true);
            vumarkPrefab.SetActive(true);
            uiCam.gameObject.SetActive(false);

            arCam.gameObject.SetActive(true);
        }
    }

    public void CloseARCamera()//ETEINDRE AR CAM
    {
        Debug.Log("Here");
        if (score >= 1)
        {
            Tuto_Manager.tuto.ActivatingTuto(3);
            Story_Manager.story.ActivateStoryInGallery(0);
        }
        if (score >= 5)//Quick le joueur pour qu'il puisse découvrir le tuto pour expliquer la récompense
        {
            Tuto_Manager.tuto.ActivatingTuto(4);
        }
        mainCanvas.worldCamera = uiCam;
        vumarkPrefab.SetActive(false);
        uiCam.gameObject.SetActive(true);
        arCam.gameObject.SetActive(false);
        ARModeMenu.SetActive(false);
        menuToActivate[currentIdxMenu].SetActive(true);
    }
    #endregion

    #region Map

    [Header("Map")]

    public Transform mapList;//Parent map list
    public Color mapColor; //Couleur de l'image sur la map
    public Image[] imageZone;//Tableaux d'image pour la map

    #endregion

    #region Menu

    [Header("Menu")]//Changer de menu

    private int currentIdxMenu = 0;//Idx du menu intro
    public GameObject[] menuToActivate;//menu à activer
    public GameObject ARModeMenu;//Menu de l'AR Mode

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
        Save_Manager.saving.SavingCrateState(idxCrateStates);
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

    #region Interface Manager
    //UI MANAGER 

    //Pour changer de menu il faut renseigner le int sur le bouton
    public void ChangeMenu(int newIdxMenu)
    {
        Debug.Log("Here");
        Audio_Manager.audio.SoundsToPlay(audioChangeMenu);
        Audio_Manager.audio.GetComponent<AudioSource>().outputAudioMixerGroup = mixerGroupChangeMenu[0];
        menuToActivate[currentIdxMenu].SetActive(false);
        menuToActivate[newIdxMenu].SetActive(true);
        currentIdxMenu = newIdxMenu;

        if (stopMusicInGallery)
        {
            DeactiveMusicMainMenu();
        }
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

    public void MapActivation(int vumarkNumber)//Maping
    {
        Debug.Log("Here");
        imageZone[vumarkNumber].color = mapColor; ;
    }

    #endregion

    #region LOAD Variable
    //LOADING VARIABLE

    public void CheckStateButton(int idx)//changement de state de la galerie
    {
        Debug.Log("Here");
        if (!scanIdx.Contains(idx))
        {
            scanIdx.Add(idx);
            if (artifactsGallery.GetChild(idx).gameObject.GetComponent<Button>().interactable == false)
            {
                artifactsGallery.GetChild(idx).gameObject.GetComponent<Button>().interactable = true;
                Save_Manager.saving.SetToTrue(idx);
                idxButton = idx;
                AddScore(1);
            }
        }
    }

    public void ButtonState(List<bool> interactableButton)//load button ok dans la galerie
    {
        Debug.Log("Here");
        for (int i = 0; i < interactableButton.Count; i++)
        {
            artifactsGallery.GetChild(i).gameObject.GetComponent<Button>().interactable = interactableButton[i];
            if (interactableButton[i] == true)
            {
                AddScore(1);
            }
        }
    }


    public void ImageState(List<bool> isImageCheck) //IMAGE ON MAP 
    {
        Debug.Log("Here");
        for (int j = 0; j < isImageCheck.Count; j++)
        {
            if (isImageCheck[j])
            {
                mapList.GetChild(j).gameObject.GetComponent<Image>().color = mapColor;
            }

        }
    }

    public void TutoIsDone(bool isTutoDone)
    {
        Debug.Log("Here");
        quizzDone = isTutoDone;
    }

    public void LoadScore(int scoring)//load score
    {
        Debug.Log("Here");
        score = scoring;
        scoreText.text = "Trésors Découverts \n" + score + " / " + limitToWin;
        currentQuestValue = score / limitToWin;
        questImage.fillAmount = currentQuestValue;
    }

    public void LoadCrateImage(List<int> crateVumarkNumber)//load coffre
    {
        Debug.Log("Here");
        idxCrateStates = crateVumarkNumber;

        for (int j = 0; j < rewardImgList.Count && crateVumarkNumber != null; j++)
        {
            rewardImgList[j].sprite = rewardSpriteList[crateVumarkNumber[j]];
        }
    }

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

    // Use this for initialization
    void Start()
    {
        Debug.Log("Here");
        scoreText.text = "Trésors Découverts \n" + score + " / " + limitToWin;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            AddScore(1);
        }
    }

    public void QuitAPK()
    {
        Application.Quit();
    } 
}
