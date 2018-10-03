using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBall : MonoBehaviour {

    public Transform spawn;
    public GameObject ball;
    private float ScoreToWin;

    public static ResetBall Instance { get; private set; }


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
    void Start ()
    {
        ScoreToWin = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("ball"))
        {
            Instantiate(ball, spawn.transform.position, spawn.transform.rotation);
            Destroy(other.gameObject);            
        }

        if (other.CompareTag("point"))
        {
            Destroy(other.gameObject);
            ScoreToWin += 1;
        }
    }

    public void ScoreToHave (int numberToWin)
    {
        ScoreToWin = numberToWin;
    }
}
