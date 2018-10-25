using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface_Manager : MonoBehaviour
{


    public static Interface_Manager Instance { get; private set; }


    [Header("Gallery")]

    public Transform galleryPannel;//gallery.GetChild(idx)
    public Transform mapList;
    private List<int> scanIdx = new List<int>();
    private List<bool> buttonState = new List<bool>();
    private int idxButton;

    [Header("Scoring")]

    public Text scoreText;
    private int score;
    public int limitToWin;

    [Header("Camera")]

    public VuforiaMonoBehaviour arCam;

    [Header("Map")]

    public GameObject[] imageZone;

    [Header("Reward")]

    public Transform spawnPointReward;
    public Text spawnPointFunFact;
    public ScriptableReward[] rewardBoard;
    public GameObject currentReward;
    //public GameObject[] rewardToSpawnBoard;
    //public string[] funFactToDisplayBoard;

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
            if(galleryPannel.GetChild(idx).gameObject.GetComponent<Button>().interactable == false)
            {
                galleryPannel.GetChild(idx).gameObject.GetComponent<Button>().interactable = true;
                Save_Manager.saving.SetToTrue(idx);
                idxButton = idx;
                AddScore(1);
            }
        }
    }

    public void ButtonState(List<bool> interactableButton)
    {
        for (int i = 0; i < interactableButton.Count; i++)// ou 41
        {
            galleryPannel.GetChild(i).gameObject.GetComponent<Button>().interactable = interactableButton[i];
            if (interactableButton[i] == true)
            {
                AddScore(1);
                Debug.Log(interactableButton[i]);
            }
        }
    }


    public void ImageState(List<bool> isCheck) 
    {
        for (int j = 0; j < isCheck.Count; j++)
        {
            mapList.GetChild(j).gameObject.GetComponent<Image>().enabled = isCheck[j];
        }
    }


    //SCORING

    public void AddScore(int newScoreValue)
    {
        score = score + newScoreValue;

        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = score +" / " + limitToWin ;
        if (score == limitToWin)
        {
            Victory();
        }
    }

    void Victory()
    {
        Debug.Log("YOU WIN !");
    }

    //CAMERA

    public void OpenARCamera()
    {
        arCam.enabled = true;
    }

    public void CloseARCamera()
    {
        ScriptTracker.Instance.OnTrackerLost();
        arCam.enabled = false;
    }

    //MAP

    public void MapActivation (int imageNumber)//Maping
    {
        Debug.Log("enabled");
        imageZone[imageNumber].GetComponent<Image>().enabled = true;
    }

    //REWARD + SAVE

    public void RewardButton(int rewardIdx) //Click sur le bouton de la galerie
    {
        currentReward = Instantiate(rewardBoard[rewardIdx].rewardToSpawn, spawnPointReward);
        spawnPointFunFact.text = rewardBoard[rewardIdx].funFactToDisplay;
        Debug.Log(transform.GetSiblingIndex());
    }

    public void DestroyRewardSpawn()
    {
        Destroy(currentReward);
    }
}
