using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBall : MonoBehaviour {

    public Transform spawn;
    public GameObject ball;
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
        if (other.CompareTag("ball"))
        {
            Destroy(other.gameObject);
            StartCoroutine(BallSpawn());
             
        }

        if (other.CompareTag("point"))
        {
            Destroy(other.gameObject);
            AddScore();
        }
    }

    IEnumerator BallSpawn ()
    {
        yield return new WaitForSeconds(2);
        Instantiate(ball, spawn.transform.position, spawn.transform.rotation);
    }

    public void AddScore ()
    {
        ScriptTracker.Instance.MiniGameScore(pointValue);
    }
}
