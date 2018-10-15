using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VumarkZoner : MonoBehaviour {

    public int mapLocation;


    public void Awake()
    {
       
    }
    // Use this for initialization
    void Start ()
    {
        Interface_Manager.Instance.MapActivation(mapLocation);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
