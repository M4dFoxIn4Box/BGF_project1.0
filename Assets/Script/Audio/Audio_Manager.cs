﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio_Manager : MonoBehaviour {

    public static Audio_Manager audio { get; private set; }

    public AudioSource sfxManager;


    private void Awake()
    {
        if (audio == null)
        {
            DontDestroyOnLoad(gameObject);
            audio = this;
        }
        else if (audio != this)
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

    public void SoundsToPlay (AudioClip currentSFX)
    {
        Debug.Log("Here");
        sfxManager.PlayOneShot(currentSFX);
    }
}
