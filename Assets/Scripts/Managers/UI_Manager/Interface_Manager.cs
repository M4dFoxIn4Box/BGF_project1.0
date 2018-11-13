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
    private float score;
    public float limitToWin;
    public GameObject victoryText;

    private float currentQuestValue;
    public Image questImage;
    public List<int> intStepToIRLReward;
    public List<GameObject> gameobjectStepToIRLReward;
    public Button buttonARMode;

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
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    //UI MANAGER 

    public void ChangeMenuPlus()
    {
        menuToActivate[currentIdxMenu].SetActive(false);
        currentIdxMenu++;
        menuToActivate[currentIdxMenu].SetActive(true);
    }

    public void ChangeMenuMoins()
    {
        menuToActivate[currentIdxMenu].SetActive(false);
        currentIdxMenu--;
        menuToActivate[currentIdxMenu].SetActive(true);
    }

    public void ShowElement(GameObject elementToActive)
    {
        elementToActive.SetActive(true);
    }

    public void UnShowElement(GameObject elementToDesactive)
    {
        elementToDesactive.SetActive(false);
    }


    //GALLERY + SAVE

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

    //SCORING

    public void AddScore(int newScoreValue)
    {
        score = score + newScoreValue;
        currentQuestValue = score / limitToWin;

        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = score +" / " + limitToWin ;
        questImage.fillAmount = currentQuestValue;

        if (currentQuestValue == intStepToIRLReward[1])
        {
            BlockARCamera();
            gameobjectStepToIRLReward.Remove(gameobjectStepToIRLReward[1]);
            intStepToIRLReward.Remove(1);
            gameobjectStepToIRLReward[1].SetActive(true);
        }

        if (score == limitToWin)
        {
            Victory();
        }
    }

    void Victory()
    {
        victoryText.SetActive(true);
    }

    public void BlockARCamera()
    {
        if(buttonARMode.interactable == true)
            buttonARMode.interactable = false;

        if (buttonARMode.interactable == false)
            buttonARMode.interactable = true;
    }

    //CAMERA

    public void OpenARCamera()
    {
        mainCanvas.worldCamera = arCam;
        menuToActivate[currentIdxMenu].SetActive(false);
        ARModeMenu.SetActive(true);
        vumarkPrefab.SetActive(true);
        uiCam.gameObject.SetActive(false);

        arCam.gameObject.SetActive(true);
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

    //MAP

    public void MapActivation (int imageNumber)//Maping
    {
        imageZone[imageNumber].GetComponent<Image>().enabled = true;
    }

}
