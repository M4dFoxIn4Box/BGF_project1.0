using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dr_Strange : MonoBehaviour {

    public Animator anim;
    public GameObject fakeARObject;
    private bool secondClick = false;
    private bool firstClick = false;


    // Use this for initialization
    void Start () {

        firstClick = true;
        ScriptTracker.Instance.FakeARToDeactivate(fakeARObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        if (firstClick == true)
        {
            anim.SetBool("Play_Anim", true);
            Debug.Log("clicked");
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
