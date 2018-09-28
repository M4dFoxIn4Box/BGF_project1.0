﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {

    public static UI_Manager instance { get; private set; }


    [Header("Gallery")]

    public GameObject gallery;

    [Header("Scoring")]

    public Text scoreText;
    private int score;

    public int[] nbxscan;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AddArtefact()
    {

    }


    public void AddScore(int newScoreValue)
    {
        score = score + newScoreValue;

        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
        if (score == 42)
        {
            Victory();
        }
    }

    public void TableauGallerie (int chien)
    {

    }

    public void ClickSwitchMenu ()
    {

    }

    void Victory ()
    {
        Debug.Log("YOU WIN");
    }
}