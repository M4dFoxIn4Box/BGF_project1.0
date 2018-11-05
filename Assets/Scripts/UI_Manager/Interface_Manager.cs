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

    [Header("Fonctionnel")]

    private int currentIdxMenu = 0;
    public GameObject[] menuToActivate;
    public GameObject ARMode;
    

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

    public void ChangeMenu(int idxMenu)
    {
        menuToActivate[currentIdxMenu].SetActive(false);
        currentIdxMenu++;
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
        menuToActivate[currentIdxMenu].SetActive(false);
        ARMode.SetActive(true);
        arCam.enabled = true;
    }

    public void CloseARCamera()
    {
        ScriptTracker.Instance.OnTrackerLost();
        arCam.enabled = false;
        ARMode.SetActive(false);
    }

    //MAP

    public void MapActivation (int imageNumber)//Maping
    {
        Debug.Log("enabled");
        imageZone[imageNumber].GetComponent<Image>().enabled = true;
    }

}
