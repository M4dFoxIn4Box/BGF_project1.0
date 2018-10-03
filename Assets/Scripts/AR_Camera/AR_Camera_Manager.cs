using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_Camera_Manager : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponent<VuforiaMonoBehaviour>().enabled = false;
        GetComponent<DefaultInitializationErrorHandler>().enabled = false;


    }

    // Update is called once per frame
    void Update () {
		
	}
}
