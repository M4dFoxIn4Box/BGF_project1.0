using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Excalibur_Interation : MonoBehaviour {

    public Animator animExcalibur;
    public int toDestroy; 

	// Use this for initialization
	void Start ()
    {
        animExcalibur = gameObject.GetComponent<Animator>();
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

    private void OnMouseDown()
    {
        
        toDestroy--;
        Debug.Log(toDestroy);

        if(toDestroy == 0)
        {
            animExcalibur.SetBool("IsTaped", true);
            Debug.Log("merde");
        }
    }
}
