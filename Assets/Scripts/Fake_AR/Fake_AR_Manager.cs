using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fake_AR_Manager : MonoBehaviour {

    public static Fake_AR_Manager FakeAR { get; private set; }

    public Transform spawnPointFakeAR;

    private void Awake()
    {
        if (FakeAR == null)
        {
            DontDestroyOnLoad(gameObject);
            FakeAR = this;
        }
        else if (FakeAR != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void FakeARToActivate(int idxFakeAR)
    {
        GameObject childList = spawnPointFakeAR.GetChild(idxFakeAR).gameObject;
        if (childList)
        {
            childList.SetActive(true);
        }
    }
}
