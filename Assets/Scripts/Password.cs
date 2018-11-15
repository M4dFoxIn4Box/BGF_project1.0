using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Password : MonoBehaviour {

    public string passwordToPass = "zeldagoty";

    public InputField passwordToEnter;
    public Text maxInputString;

    private string[] maskArray = new string[18];
    private int maxIndex;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void PasswordToCheck()
    {

        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            maxIndex--;
            maxInputString.text = maskArray[maxIndex];
        }
        else
        {
            maxIndex++;
            maxInputString.text = maskArray[maxIndex];
        }
        //if(passwordToPass == "zeldagoty")
        //{
        //    Debug.Log("BRAVO LE VEAU");
        //}
    }
}
