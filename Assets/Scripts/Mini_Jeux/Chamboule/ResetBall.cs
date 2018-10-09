using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBall : MonoBehaviour {

    private int pointValue;

    // Use this for initialization
    void Start ()
    {
        pointValue = 1;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("point"))
        {
            Destroy(other.gameObject);
            AddScore();
        }
    }


    public void AddScore ()
    {
        ScriptTracker.Instance.MiniGameScore(pointValue);
    }
}
