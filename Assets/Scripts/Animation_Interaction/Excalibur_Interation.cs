using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Excalibur_Interation : MonoBehaviour {

    private Animator currentAnimation;
    private bool isOKTap = true;
    public GameObject fakeARObject;
    
	// Use this for initialization
	void Start ()
    {
        ScriptTracker.Instance.FakeARToDeactivate(fakeARObject);
        currentAnimation = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        
    }

    void OnMouseDown()
    {
        if(isOKTap)
        {
            isOKTap = false;
            currentAnimation.SetTrigger("Excalibur_Make_Step");
        }
    }

    public void PlayerCanTap()
    {
        isOKTap = true;
    }

    //public void ExcaliburIsFinished()
    //{
        
    //    Debug.Log(fakeARObject);
    //}
}
