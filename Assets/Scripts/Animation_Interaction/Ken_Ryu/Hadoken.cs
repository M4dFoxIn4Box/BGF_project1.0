using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hadoken : MonoBehaviour {

    public Animator anim;
    public Animator ryuAnim;
    public GameObject fakeARObject;

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
        anim.SetBool("Hadoken", true);
        Debug.Log("chage state0");
    }

    void DeathActive ()
    {
        ryuAnim.SetBool("Death_Hadoken", true);
    }
}
