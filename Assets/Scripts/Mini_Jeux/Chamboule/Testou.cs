using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testou : MonoBehaviour {

    public GameObject miniJeu;

    public static Testou Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Use this for initialization
    void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Tesmort ()
    {
        Debug.Log("zz");
        Destroy(miniJeu);
    }
}
