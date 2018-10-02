using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptQuizz : MonoBehaviour {

    public Image questionBackground;
    public Image screenBackground;

    public Canvas quizzCanvas;
    public GameObject quizzOnlyPanel;
    public Button leaveCanvas;

    public Text quizzText;
    public Text answer1;
    public Text answer2;
    public Text answer3;
    public Text answer4;
    public Text funFact;

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    private float r = 0.4509804f;
    private float g = 0.4509804f;
    private float b = 0.4509804f;

    private string answer1Text;
    private string answer2Text;
    private string answer3Text;
    private string answer4Text;
    private string funFactText;

    public GameObject congratulationsImage;
    public Image trophyImage;

    private bool isAnswered = false;

    public ScriptableQuizz scriptableQuizzClass;

    // Use this for initialization
    void Start ()
    {

        quizzText.text = scriptableQuizzClass.quizzQuestion;
        answer1Text = scriptableQuizzClass.answer1;
        answer2Text = scriptableQuizzClass.answer2;
        answer3Text = scriptableQuizzClass.answer3;
        answer4Text = scriptableQuizzClass.answer4;
        funFactText = scriptableQuizzClass.funFact;

        answer1.text = answer1Text;
        answer2.text = answer2Text;
        answer3.text = answer3Text;
        answer4.text = answer4Text;
        funFact.text = funFactText;

        button1.onClick.AddListener(TaskOnClick1);
        button2.onClick.AddListener(TaskOnClick2);
        button3.onClick.AddListener(TaskOnClick3);
        button4.onClick.AddListener(TaskOnClick4);

        leaveCanvas.onClick.AddListener(LeaveQuizz);

    }

	
    public void TaskOnClick1() // BOUTON 1
    {
        if (scriptableQuizzClass.rightAnswer == 1)
        {
            button1.GetComponent<Image>().color = Color.green;
            button2.GetComponent<Image>().color = Color.red;
            button3.GetComponent<Image>().color = Color.red;
            button4.GetComponent<Image>().color = Color.red;

            RightAnswer();
        }
        else
        {
            button1.GetComponent<Image>().color = Color.red;
            button1.interactable = false;
        }
    }

    public void TaskOnClick2() // BOUTON 2
    {
        if (scriptableQuizzClass.rightAnswer == 2)
        {
            button1.GetComponent<Image>().color = Color.red;
            button2.GetComponent<Image>().color = Color.green;
            button3.GetComponent<Image>().color = Color.red;
            button4.GetComponent<Image>().color = Color.red;

            RightAnswer();
        }
        else
        {
            button2.GetComponent<Image>().color = Color.red;
            button2.interactable = false;
        }
    }

    public void TaskOnClick3() // BOUTON 3
    {
        if (scriptableQuizzClass.rightAnswer == 3)
        {
            button1.GetComponent<Image>().color = Color.red;
            button2.GetComponent<Image>().color = Color.red;
            button3.GetComponent<Image>().color = Color.green;
            button4.GetComponent<Image>().color = Color.red;

            RightAnswer();
        }
        else
        {
            button3.GetComponent<Image>().color = Color.red;
            button3.interactable = false;
        }
    }

    public void TaskOnClick4() // BOUTON 4
    {
        if (scriptableQuizzClass.rightAnswer == 4)
        {
            button1.GetComponent<Image>().color = Color.red;
            button2.GetComponent<Image>().color = Color.red;
            button3.GetComponent<Image>().color = Color.red;
            button4.GetComponent<Image>().color = Color.green;

            RightAnswer();
        }
        else
        {
            button4.GetComponent<Image>().color = Color.red;
            button4.interactable = false;
        }
    }


    public void LeaveQuizz()
    {
        quizzCanvas.enabled = false;

        if (isAnswered == true)
        {
            quizzOnlyPanel.SetActive(false);
            screenBackground.enabled = false;
            

        }

        button1.GetComponent<Image>().color = new Color(r,g,b);
        button2.GetComponent<Image>().color = new Color(r,g,b);
        button3.GetComponent<Image>().color = new Color(r,g,b);
        button4.GetComponent<Image>().color = new Color(r,g,b);

        questionBackground.color = new Color(0.7254902f, 0.7254902f, 0.7254902f);

        button1.interactable = true;
        button2.interactable = true;
        button3.interactable = true;
        button4.interactable = true;
    }

    public void RightAnswer()
    {
        button1.interactable = false;
        button2.interactable = false;
        button3.interactable = false;
        button4.interactable = false;

        congratulationsImage.SetActive(true);

        questionBackground.color = Color.green;

        isAnswered = true;

        StartCoroutine(WaitForSeconds());
        
    }

    IEnumerator WaitForSeconds()
    {

        yield return new WaitForSeconds(2);
        quizzOnlyPanel.SetActive(false);
    }


    void Update () {

        if (Input.GetKeyDown("q"))
        {

            quizzCanvas.enabled = true;
            screenBackground.enabled = true;

        }

    }
}
