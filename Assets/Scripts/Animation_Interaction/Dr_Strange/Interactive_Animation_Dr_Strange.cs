using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Interactive_Animation_Dr_Strange : MonoBehaviour {

    public Animator anim;
    public GameObject fakeARObject;
    private bool secondClick = false;
    private bool firstClick = false;

    public AudioClip[] audioDrStrange;
    public AudioMixerGroup[] mixerDrStrange;


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
