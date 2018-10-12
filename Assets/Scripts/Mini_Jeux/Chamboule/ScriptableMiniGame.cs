using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniJeux", menuName = "Scriptable/MiniJeux", order = 1)]

public class ScriptableMiniGame : ScriptableObject
{

    public GameObject prefabMiniJeux;
    public int scoreLimit;

    public bool hasBeenDone;
    public GameObject miniGameReward;
    public string miniGameFunFact;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
