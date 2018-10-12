using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface_Manager : MonoBehaviour
{


    public static Interface_Manager Instance { get; private set; }


    [Header("Gallery")]

    public Transform galleryPannel;//gallery.GetChild(idx)
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
    
    public int[] vumarkByZone;
    public Image[] imageZone;

    [Header("Reward")]

    public Transform spawnPointReward;
    public Text spawnPointFunFact;
    public GameObject[] rewardToSpawnBoard;
    public string[] funFactToDisplayBoard;

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
            }
        }
    }

    public void ButtonState(List<bool> isTrue)
    {
        for (int i = 0; i < isTrue.Count; i++)// ou 41
        {   
            galleryPannel.GetChild(i).gameObject.GetComponent<Button>().interactable = isTrue[i];
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

    /*public void Zone1 (int vumarkInZone1)
    {
        vumarkByZone[0] = vumarkInZone1 + vumarkByZone[0];
        Debug.Log(vumarkByZone[0]);
    }

    public void Zone2(int vumarkInZone2)
    {

    }

    public void Zone3(int vumarkInZone3)
    {

    }

    public void Zone4(int vumarkInZone4)
    {

    }

    public void Zone5(int vumarkInZone5)
    {

    }

    public void Zone6(int vumarkInZone6)
    {

    }*/

    public void RewardButton()
    {
        Instantiate(rewardToSpawnBoard[idxButton], spawnPointReward);
        spawnPointFunFact.text = funFactToDisplayBoard[idxButton];
    }

    public void RewardManager(GameObject rewardSpawn, string funFactDisplay)
    {
        rewardToSpawnBoard[idxButton] = rewardSpawn;
        funFactToDisplayBoard[idxButton] = funFactDisplay;
        //Save_Manager.saving.RewardSave(rewardToSpawnBoard, funFactToDisplayBoard);
    }
}
