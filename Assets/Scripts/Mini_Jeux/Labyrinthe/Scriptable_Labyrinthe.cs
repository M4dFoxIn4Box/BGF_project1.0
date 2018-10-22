using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniJeux", menuName = "Scriptable/Labyrinthe", order = 2)]

public class Scriptable_Labyrinthe : ScriptableObject
{

    public GameObject prefabMiniJeux;
    public GameObject miniGameReward;
    public bool hasBeenDone;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
