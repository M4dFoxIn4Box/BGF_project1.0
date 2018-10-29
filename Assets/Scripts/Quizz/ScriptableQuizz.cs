using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Quizz", menuName = "Scriptable/Quizz", order = 1)]

public class ScriptableQuizz : ScriptableObject {
        
    public string quizzName;
    public string quizzQuestion;
    public string[] answerList;

    [Tooltip("1 correspond à answer1, etc")]
    public int rightAnswer; // 1 à 4, 1 correspondant à answer1, etc...

}
