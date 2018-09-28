using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptQuizz : MonoBehaviour {

    public Text quizzText;
    public Text answer1;
    public Text answer2;
    public Text answer3;
    public Text answer4;

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    private string answer1Text;
    private string answer2Text;
    private string answer3Text;
    private string answer4Text;

    //public string questionText;
    public ScriptableQuizz scriptableQuizzClass;

    // Use this for initialization
    void Start () {

        quizzText.text = scriptableQuizzClass.quizzQuestion;
        answer1Text = scriptableQuizzClass.answer1;
        answer2Text = scriptableQuizzClass.answer2;
        answer3Text = scriptableQuizzClass.answer3;
        answer4Text = scriptableQuizzClass.answer4;

        answer1.text = answer1Text;
        answer2.text = answer2Text;
        answer3.text = answer3Text;
        answer4.text = answer4Text;

        button1.onClick.AddListener(taskOnClick1);
        button2.onClick.AddListener(taskOnClick2);
        button3.onClick.AddListener(taskOnClick3);
        button4.onClick.AddListener(taskOnClick4);


    }

	
    public void taskOnClick1() // BOUTON 1
    {
        if (scriptableQuizzClass.rightAnswer == 1)
        {
            button1.GetComponent<Image>().color = Color.green;
            button2.GetComponent<Image>().color = Color.red;
            button3.GetComponent<Image>().color = Color.red;
            button4.GetComponent<Image>().color = Color.red;
        }
        else
        {
            button1.GetComponent<Image>().color = Color.red;
        }
    }

    public void taskOnClick2() // BOUTON 2
    {
        if (scriptableQuizzClass.rightAnswer == 2)
        {
            button1.GetComponent<Image>().color = Color.red;
            button2.GetComponent<Image>().color = Color.green;
            button3.GetComponent<Image>().color = Color.red;
            button4.GetComponent<Image>().color = Color.red;
        }
        else
        {
            button2.GetComponent<Image>().color = Color.red;
        }
    }

    public void taskOnClick3() // BOUTON 3
    {
        if (scriptableQuizzClass.rightAnswer == 3)
        {
            button1.GetComponent<Image>().color = Color.red;
            button2.GetComponent<Image>().color = Color.red;
            button3.GetComponent<Image>().color = Color.green;
            button4.GetComponent<Image>().color = Color.red;
        }
        else
        {
            button3.GetComponent<Image>().color = Color.red;
        }
    }

    public void taskOnClick4() // BOUTON 4
    {
        if (scriptableQuizzClass.rightAnswer == 4)
        {
            button1.GetComponent<Image>().color = Color.red;
            button2.GetComponent<Image>().color = Color.red;
            button3.GetComponent<Image>().color = Color.red;
            button4.GetComponent<Image>().color = Color.green;
        }
        else
        {
            button4.GetComponent<Image>().color = Color.red;
        }
    }

    void Update () {
		
	}
}
