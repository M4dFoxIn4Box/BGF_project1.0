using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizzManager_Category", menuName = "Scriptable/Quizz Manager", order = 1)]

public class ScriptableQuizzManager : ScriptableObject
{
    public List<ScriptableQuizz> scriptableQuizzList;
    public string funFact;
    public GameObject rewardToSpawn;
    public int errorLimit;
    public int scoreToWin;

}
