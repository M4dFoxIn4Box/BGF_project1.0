using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSManager : MonoBehaviour {

    public Text fpsTxt;
    private float fpsValue;
    private bool isCalculating;

    private void Start()
    {
        fpsValue = 1.0f / Time.deltaTime;
        fpsTxt.text = fpsValue.ToString("F0");
    }

    // Update is called once per frame
    void Update ()
    {
        if(!isCalculating)
        {
            isCalculating = true;
            fpsValue = 1.0f / Time.deltaTime;
            StartCoroutine(FPSDebug());            
        }
       
      
	}

    IEnumerator FPSDebug ()
    {
        yield return new WaitForSeconds(1f);
        fpsTxt.text = fpsValue.ToString("F0");
        isCalculating = false;
    }
}
