using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour {

    public Animator anim;
    public GameObject fakeARObject;
    public GameObject champignon;

    // Use this for initialization
    void Start () {
        ScriptTracker.Instance.FakeARToDeactivate(fakeARObject);
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        anim.SetBool("AnimPlay", true);
    }

    void ActiveChampi()
    {
        champignon.SetActive(true);
    }
}
