using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dr_Strange : MonoBehaviour {

    private Animator anim;
    public GameObject fakeARObject;
    private bool secondClick = false;
    private bool firstClick = false;


    // Use this for initialization
    void Start () {

        ScriptTracker.Instance.FakeARToDeactivate(fakeARObject);
        anim = GetComponent<Animator>();
        firstClick = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        if (firstClick == true)
        {
            anim.SetBool("Play_Anim", true);
        }

        if (secondClick == true)
        {
            anim.SetBool("Reverse", true);
        }
    }


    void PlayerCanRetape()
    {
        secondClick = true;
    }
}
