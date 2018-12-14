using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joker_Script : MonoBehaviour {

    public Animator jokerAnimator;
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        jokerAnimator.SetBool("Batarang", true);
    }
}
