using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Diode_Activation : MonoBehaviour {

    public Image diodeToActive;
    public Image diodeGauge;

    public Color activatedColor;
    private bool loadGauge = false;
    private float timer = 2f;
	// Use this for initialization
	void Start ()
    {
       
    }

    void Update()
    {
        if(loadGauge == true && diodeGauge.fillAmount != 1)
        {
            diodeGauge.fillAmount += Time.deltaTime / timer;
            if (diodeGauge.fillAmount == 1)
            {
                diodeToActive.color = activatedColor;
            }
       }
    }

    // Update is called once per frame
    public void DiodeActivation ()
    {
        if (loadGauge == false)
        {
            loadGauge = true;
            Debug.Log(loadGauge);
        }

	}
}
