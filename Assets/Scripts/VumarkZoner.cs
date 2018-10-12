using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VumarkZoner : MonoBehaviour {

    public enum justeZone { zone1, zone2, zone3, zone4, zone5, zone6 }

    public justeZone zoneType;


    public void Awake()
    {
        //Interface_Manager.Instance.Zone1((int)zoneType);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
