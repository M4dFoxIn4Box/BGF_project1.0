using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Input : MonoBehaviour {

    public GameObject tracer;
    public Vector3 velocity;


    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
    if (Input.GetButtonDown("a"))
        {
            tracer.SetActive(false);
        }

    }
}
