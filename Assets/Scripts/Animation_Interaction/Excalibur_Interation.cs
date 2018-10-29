using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Excalibur_Interation : MonoBehaviour {

    // Animator animExcalibur;
    private int toDestroy;
    private Animation currentAnimation;

	// Use this for initialization
	void Start ()
    {
        //animExcalibur = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        
    }

    //public void OnCollisoonEnter(Collision other)
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        toDestroy--;
    //        Debug.Log(toDestroy);
    //        if (toDestroy == 0)
    //        {
    //            animExcalibur.SetTrigger("isTaped");
    //        }
    //    }
    //}

    //private void onmousedown()
    //{
    //    todestroy--;
    //    debug.log(todestroy);

    //    if (todestroy == 0)
    //    {
    //        animexcalibur.setbool("istaped", true);
    //        debug.log("merde");
    //    }
    //}

    public void PauseAnimation(Animation animation)
    {
        animation = currentAnimation;
        Debug.Log("ET DE 1");
        currentAnimation["Excalibur_Anime"].speed = 0;
        Debug.Log("ET DE 2");
        //currentAnimation.enabled = false;
        OnMouseDown();
    }

    void OnMouseDown()
    {
        Debug.Log("OH HISSE !");
        currentAnimation["Excalibur_Anime"].speed = 1;

    }
}
