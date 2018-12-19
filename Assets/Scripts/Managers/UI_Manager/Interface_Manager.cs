﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface_Manager : MonoBehaviour
{
    public static Interface_Manager Instance { get; private set; }


    [Header("Main Gallery")]

    public Transform artifactsGallery;
    private List<int> scanIdx = new List<int>();
    private List<bool> buttonState = new List<bool>();
    private int idxButton;
    public GameObject funfact;
    private bool isFunFactActive;
    public Image imageFunfactState;
    public Sprite[] spriteFunfactState;

    [Header ("Map")]

    public Transform mapList;

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

    [Header("Camera")]

    public Camera arCam;//AR Camera
    public Camera uiCam;//UI Camera
    public Canvas mainCanvas;//Main canvas

    [Header ("AR Mode")]

    public Button buttonARMode;//Buton AR Mode
    public GameObject vumarkPrefab;//Vumark to activate/deactivate

    [Header("Map")]

    public Color mapColor;
    public Image[] imageZone;//Tableaux d'image pour la map

    [Header("Menu")]//Changer de menu

    private int currentIdxMenu = 1;//Idx du menu intro
    public GameObject[] menuToActivate;//menu à activer
    public GameObject ARModeMenu;//Menu de l'AR Mode

    [Header("Tutoriel quizz")]

    public int tutoQuizzIdx;//l'index du quizz
    public bool quizzDone;//bool si le tuto à été fait et qui envoyé au save manager

    [Header("Récompenses & Box")]

    public List<bool> rewardAlreadyDone;//si le coffre à été récupéré
    public List<int> idxCrateStates;//index du coffre
    private int rewardCounter;//

    [Header("Story")]

    public List<int> idxStoryScriptableToActivate;//index à envoyé pour activer la bonne histoire
    private bool storyToActivate = false;

    [Header("Sounds")]

    public AudioClip audioChangeMenu;

    public AudioSource musicMainMenuToDeactivate;
    private bool stopMusicInGallery = false;

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
        scoreText.text = "Artéfacts Découverts \n" + score + " / " + limitToWin;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            AddScore(1);
        }
    }


    //UI MANAGER 

    //Pour changer de menu il faut renseigner le int sur le bouton
    public void ChangeMenu(int newIdxMenu)
    {
        Audio_Manager.audio.SoundsToPlay(audioChangeMenu);
        menuToActivate[currentIdxMenu].SetActive(false);
        menuToActivate[newIdxMenu].SetActive(true);
        currentIdxMenu = newIdxMenu;

        if(stopMusicInGallery)
        {
            DeactiveMusicMainMenu();
        }
    }

    public void ShowElement(GameObject elementToActive)
    {
        elementToActive.SetActive(true);
    }

    public void UnShowElement(GameObject elementToDesactive)
    {
        elementToDesactive.SetActive(false);
    }

    public void QuitAPK()
    {
        Application.Quit();
    }

    public void DeactiveMusicMainMenu()//Musique à désactiver ou activer dans la galerie
    {
        if(stopMusicInGallery)
        {
            musicMainMenuToDeactivate.UnPause();
            stopMusicInGallery = false;
        }
        else if(!stopMusicInGallery)
        {
            musicMainMenuToDeactivate.Pause();
            stopMusicInGallery = true;
        }
    }
    //AUDIO 


    //LOADING VARIABLE

    public void CheckStateButton(int idx)//changement de state de la galerie
    {
        if (!scanIdx.Contains(idx))
        {
            scanIdx.Add(idx);
            if(artifactsGallery.GetChild(idx).gameObject.GetComponent<Button>().interactable == false)
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
        for (int j = 0; j < isImageCheck.Count; j++)
        {
            if(isImageCheck[j])
            {
                mapList.GetChild(j).gameObject.GetComponent<Image>().color = mapColor;
            }
            
        }
    }

    public void TutoIsDone(bool isTutoDone)
    {
        quizzDone = isTutoDone;
    }

    //LOADING SCORE

    public void LoadScore(int scoring)//load score
    {
        score = scoring;
        scoreText.text = "Artéfacts Découverts \n" + score + " / " + limitToWin;
        currentQuestValue = score / limitToWin;
        questImage.fillAmount = currentQuestValue;
    }

    public void LoadCrateImage(List<int> crateVumarkNumber)//load coffre
    {
        for (int j = 0; j < rewardImgList.Count && crateVumarkNumber != null; j++)
        {
            rewardImgList[j].sprite = rewardSpriteList[crateVumarkNumber[j]];
        }
    }

    //SCORING & PALIER

    public void AddScore(int newScoreValue)
    {
        score = score + newScoreValue;
        currentQuestValue = score / limitToWin;
        scoreText.text = "Artéfacts Découverts \n" + score + " / " + limitToWin;
        questImage.fillAmount = currentQuestValue;
        Save_Manager.saving.SavingScore((int)score);
        UpdateScore();
    }

    void UpdateScore()
    {   
         if (score == palierScoreList[0])
         {
            rewardImgList[rewardCounter].sprite = rewardSpriteList[1];
            idxCrateStates[rewardCounter] = 1;

            Save_Manager.saving.SavingCrateState(idxCrateStates);
            rewardCounter++;

            Story_Manager.story.ActivateStoryInGallery(idxStoryScriptableToActivate[0]);
            //idxStoryScriptableToActivate.RemoveAt(0);

            //palierScoreList.RemoveAt(0);
         }

        if (score == palierScoreList[1])
        {
            rewardImgList[rewardCounter].sprite = rewardSpriteList[1];
            idxCrateStates[rewardCounter] = 1;

            Save_Manager.saving.SavingCrateState(idxCrateStates);
            rewardCounter++;

            Story_Manager.story.ActivateStoryInGallery(idxStoryScriptableToActivate[1]);
        }
        if (score == palierScoreList[2])
        {
            rewardImgList[rewardCounter].sprite = rewardSpriteList[1];
            idxCrateStates[rewardCounter] = 1;

            Save_Manager.saving.SavingCrateState(idxCrateStates);
            rewardCounter++;

            Story_Manager.story.ActivateStoryInGallery(idxStoryScriptableToActivate[2]);
        }


        //if (score == limitToWin)
        //{
        //    Victory();
        //}
    }

    public void RewardBoxOpened(int rewardBoxIdx)
    {
        rewardImgList[rewardBoxIdx].sprite = rewardSpriteList[2];
        idxCrateStates[rewardBoxIdx] = 2;
        Save_Manager.saving.SavingCrateState(idxCrateStates);
    }

    //void Victory()
    //{
    //    victoryText.SetActive(true);
    //}

    //CAMERA

    public void OpenARCamera()//ALLUMER L AR CAM
    {
        if(quizzDone == false)
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
        if (score >= 1)
        {
            Tuto_Manager.tuto.ActivatingTuto(3);
            Story_Manager.story.ActivateStoryInGallery(0);
        }
        if (score >= 5)//Quick le joueur pour qu'il puisse découvrir le tuto pour expliquer la récompense
        {
            Tuto_Manager.tuto.ActivatingTuto(4);
        }


    }

    //MAP MENU UPDATE

    public void MapActivation (int vumarkNumber)//Maping
    {
        imageZone[vumarkNumber].color = mapColor; ;
    }

    public void FunFactToggle()
    {
        if (funfact.activeSelf == true)
        {
            funfact.SetActive(false);
            imageFunfactState.sprite = spriteFunfactState[0];
        }
        else if(funfact.activeSelf == false)
        {
            funfact.SetActive(true);
            imageFunfactState.sprite = spriteFunfactState[1];
        }
    }
}
