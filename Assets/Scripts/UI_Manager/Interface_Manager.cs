using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface_Manager : MonoBehaviour
{


    public static Interface_Manager Instance { get; private set; }


    [Header("Gallery")]

    public Transform galleryPannel;//gallery.GetChild(idx)
    public Button buttonGallery;
    private List<int> scanIdx = new List<int>();
    private List<bool> buttonState = new List<bool>();

    [Header("Scoring")]

    public Text scoreText;
    private int score;
    public int limitToWin;

    [Header("Camera")]

    public VuforiaMonoBehaviour arCam;



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
            if(galleryPannel.GetChild(idx).gameObject.GetComponent<Button>().interactable = false)
            {
                galleryPannel.GetChild(idx).gameObject.GetComponent<Button>().interactable = true;
                Save_Manager.saving.Save();
                Save_Manager.saving.transform.SetSiblingIndex(idx);
            }
        }
    }

    public void ButtonState(List<bool> isTrue)
    {
        for (int i = 0; i < 42; i++)// ou 41
        {
            //galleryPannel.GetChild(isTrue).gameObject.GetComponent<Button>().interactable();
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
        arCam.enabled = false;
    }

}
