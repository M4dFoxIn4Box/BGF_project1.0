using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBall : MonoBehaviour {

    public Transform spawn;
    public GameObject ball;
    private float ScoreToWin;
    public GameObject miniJeu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (ScoreToWin == 4)
        {
            miniJeu.SetActive(false);
        }
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("ball"))
        {
            Destroy(other.gameObject);
            Instantiate(ball, spawn.transform.position, spawn.transform.rotation);
        }

        if (other.CompareTag("point"))
        {
            Destroy(other.gameObject);
            ScoreToWin += 1;
        }
    }
}
