using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring_Item : MonoBehaviour {

    public int valueToAdd;

	// Use this for initialization
	void Start ()
    {
        UI_Manager.Instance.AddScore(valueToAdd);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
