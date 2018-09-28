using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptQuizz : MonoBehaviour {

    public Text quizzText;
    public string questionText;

    // Use this for initialization
    void Start () {

        quizzText.text = questionText;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
