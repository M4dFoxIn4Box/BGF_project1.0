using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Excalibur_Interation : MonoBehaviour {

    private Animator currentAnimation;
    private bool isOKTap = true;

	// Use this for initialization
	void Start ()
    {
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
        //il faut créer un bool et un event lié à lui
    }
}
