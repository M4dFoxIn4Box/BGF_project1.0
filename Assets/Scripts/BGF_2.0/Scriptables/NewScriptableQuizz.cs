using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Quizz", menuName = "Scriptable/VuMark", order = 1)]

public class NewScriptableQuizz : ScriptableObject
{
    [Header("Quizz")]

    public string quizzName = "newQuizz";
    public string quizzQuestion;
    public string answer1;
    public string answer2;
    public string answer3;
    public string answer4;

    public GameObject rewardObj;

    public string funFact;
    [Tooltip("1 correspond à answer1, etc")]
    public int rightAnswer; // 1 à 4, 1 correspondant à answer1, etc...

    public string vuMarkType;


    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {

	}
}
