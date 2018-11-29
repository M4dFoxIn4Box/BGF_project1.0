using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface_Manager : MonoBehaviour
{


    public static Interface_Manager Instance { get; private set; }


    [Header("Gallery")]

    public Transform artifactsGallery;//gallery.GetChild(idx)
    public Transform mapList;
    private List<int> scanIdx = new List<int>();
    private List<bool> buttonState = new List<bool>();
    private int idxButton;

    [Header("Scoring")]

    public Text scoreText;
    public float score;
    public float limitToWin;
    public GameObject victoryText;

    private float currentQuestValue;
    public Image questImage;
    public List<int> palierScoreList;
    public List<Image> rewardImgList;
    public List<Sprite> rewardSpriteList;
    public List<string> palierPasswordList;
    public Button buttonARMode;

    [Header("Password")]

    public GameObject inputfieldToActivate;
    public InputField passwordField;

    [Header("Clues")]

    public List<GameObject> cluesListGameobject;
    public List<string> cluesListText;
    public int cluesScore;

    [Header("Camera")]

    public Camera arCam;
    public Camera uiCam;
    public Canvas mainCanvas;

    [Header ("AR Mode")]

    public GameObject vumarkPrefab;

    [Header("Map")]

    public GameObject[] imageZone;

    [Header("Fonctionnel")]

    private int currentIdxMenu = 1;
    public GameObject[] menuToActivate;
    public GameObject ARModeMenu;

    [Header("Tutoriel quizz")]

    public int tutoQuizzIdx;
    public bool quizzDone;

    [Header("Récompenses")]

    public string textToShow;
    public List<bool> rewardAlreadyDone;

    [Header("Story")]

    public List<int> idxStoryScriptableToActivate;


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
        //for (int i = (int)score; (int)score > palierScoreList[0];)
        //{
        //    rewardSprite.RemoveAt(0);
        //    palierScoreList.RemoveAt(0);
        //    rewardSprite[0].SetActive(true);
        //}

        //for (int i = 0; i < cluesScore; i++)
        //{
        //    cluesListGameobject[0].GetComponent<Text>().text = cluesListText[0];
        //    cluesListGameobject.RemoveAt(0);
        //    cluesListText.RemoveAt(0);
        //}

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
        menuToActivate[currentIdxMenu].SetActive(false);
        menuToActivate[newIdxMenu].SetActive(true);
        currentIdxMenu = newIdxMenu;
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


    //LOADING VARIABLE

    public void CheckStateButton(int idx)
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

    public void ButtonState(List<bool> interactableButton)
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


    public void ImageState(List<bool> isImageCheck) 
    {
        for (int j = 0; j < isImageCheck.Count; j++)
        {
            mapList.GetChild(j).gameObject.GetComponent<Image>().enabled = isImageCheck[j];
        }
    }

    public void TutoIsDone(bool isTutoDone)
    {
        quizzDone = isTutoDone;
    }

    public void LoadClueScore(int scoreClue)
    {
        scoreClue = cluesScore;
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
        if (score != palierScoreList[0])
        {
            CluesManager();
            if(score == (palierScoreList[0]))
            {
                Debug.Log("Bien joué");
            }
        }
        else if (score == palierScoreList[0])
        {
            //rewardSprite[0].SetActive(true);
            //rewardSprite.RemoveAt(0);
            rewardImgList[0].sprite = rewardSpriteList[0];
            rewardImgList.RemoveAt(0);
            palierScoreList.RemoveAt(0);
            Story_Manager.story.ActivatingStory(idxStoryScriptableToActivate[0]);
            idxStoryScriptableToActivate.RemoveAt(0);
        }

        if (score == limitToWin)
        {
            Victory();
        }

        if(score == 5)
        {
            CloseARCamera();
            Tuto_Manager.tuto.ActivatingTuto(5);
        }
    }

    public void RewardBoxOpened(int rewardBoxIdx)
    {
        rewardImgList[rewardBoxIdx].sprite = rewardSpriteList[1];
        Debug.Log(rewardBoxIdx);
    }

    void Victory()
    {
        victoryText.SetActive(true);
    }

    //SAVE & LOAD SCORE

    public void LoadScore(int scoring)
    {
        score = scoring;
        scoreText.text = "Artéfacts Découverts \n" + score + " / " + limitToWin;
        currentQuestValue = score / limitToWin;
        questImage.fillAmount = currentQuestValue;
    }

    //public void BlockARCamera()
    //{
    //    if (buttonARMode.interactable == true)
    //    {
    //        buttonARMode.interactable = false;
    //        
    //    }
    //    else
    //    {
    //        buttonARMode.interactable = true;
    //    }
    //}

    //REWARD TO BLOCK

    public void CluesManager()
    {
        cluesScore++;
        Debug.Log("Clues score = " + cluesScore);
        Save_Manager.saving.ScoreClue(cluesScore);
        cluesListGameobject[0].GetComponent<Text>().text = cluesListText[0];
        cluesListGameobject.RemoveAt(0);
        cluesListText.RemoveAt(0);
    }

    //PASSWORD

    public void PasswordToCheck()
    {
        if (passwordField.text == palierPasswordList[0])
        {
            //BlockARCamera();
            inputfieldToActivate.SetActive(false);
            palierPasswordList.RemoveAt(0);
            passwordField.text = "";
            //ScriptTracker.Instance.ARLocker();
        }
    }

    //CAMERA

    public void OpenARCamera()
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

    public void CloseARCamera()
    {
        mainCanvas.worldCamera = uiCam;
        vumarkPrefab.SetActive(false);
        uiCam.gameObject.SetActive(true);
        arCam.gameObject.SetActive(false);
        ARModeMenu.SetActive(false);
        menuToActivate[currentIdxMenu].SetActive(true);
    }

    //MAP MENU UPDATE

    public void MapActivation (int imageNumber)//Maping
    {
        imageZone[imageNumber].GetComponent<Image>().enabled = true;
    }

}
