using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour {

    public GameObject ball;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter (Collider ball)
    {

        if(ball.CompareTag("ball"))
        {
            ScriptTracker.Instance.MiniGameScore();
        }
    }
}
