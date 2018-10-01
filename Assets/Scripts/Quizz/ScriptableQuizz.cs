using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizzQuestion", menuName = "Inventory/Question", order = 1)]

public class ScriptableQuizz : ScriptableObject {
        
    public string quizzName = "newQuizz";
    public string quizzQuestion;
    public string answer1;
    public string answer2;
    public string answer3;
    public string answer4;

    public string funFact;
    [Tooltip("1 correspond à answer1, etc")]
    public int rightAnswer; // 1 à 4, 1 correspondant à answer1, etc...

	// Use this for initialization
	void Start () {
		


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
