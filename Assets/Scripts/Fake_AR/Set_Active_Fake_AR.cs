using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Active_Fake_AR : MonoBehaviour {

    public int idxVumark;

	// Use this for initialization
	void Start ()
    {
        Fake_AR_Manager.FakeAR.FakeARToActivate(idxVumark);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
