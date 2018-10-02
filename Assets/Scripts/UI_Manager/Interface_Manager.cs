using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface_Manager : MonoBehaviour
{

    public static Interface_Manager Instance { get; private set; }


    [Header("Gallery")]

    public Transform gallery;//gallery.GetChild(idx)
    private List<int> scanIdx = new List<int>();

    [Header("Scoring")]

    public Text scoreText;
    private int score;
    public int limitToWin;


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

    public void ShowElement(GameObject elementToActive)
    {
        elementToActive.SetActive(true);
    }

    public void UnShowElement(GameObject elementToDesactive)
    {
        elementToDesactive.SetActive(false);
    }

    public void FillInScanIdx(int idx)
    {
        if (!scanIdx.Contains(idx))
        {
            scanIdx.Add(idx);
            gallery.GetChild(idx).gameObject.SetActive(true);
        }
    }


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

    public void TableauGallerie(int chien)
    {

    }

    public void ClickSwitchMenu()
    {

    }

    void Victory()
    {
        Debug.Log("YOU WIN !");
    }


}
