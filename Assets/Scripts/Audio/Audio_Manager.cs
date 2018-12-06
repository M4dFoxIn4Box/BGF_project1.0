using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour {

    public AudioSource sfxManager;

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
        sfxManager.PlayOneShot(currentSFX);
    }
}
