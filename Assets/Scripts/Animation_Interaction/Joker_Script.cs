using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joker_Script : MonoBehaviour {

    public Animator jokerAnimator;
    public GameObject fakeARObject;

    // Use this for initialization
    void Start () {
        ScriptTracker.Instance.FakeARToDeactivate(fakeARObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        jokerAnimator.SetBool("Batarang", true);
    }
}
