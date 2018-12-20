using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fake_AR_Object_To_Destroy : MonoBehaviour {

    public GameObject fakeARObject;

    // Use this for initialization
    void Start ()
    {
        ScriptTracker.Instance.FakeARToDeactivate(fakeARObject);
    }
	
}
