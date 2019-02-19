using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public static AudioManager s_Singleton { get; private set; }

    private AudioSource myAudioSource;
    
    private void Awake()
    {
        if (s_Singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            s_Singleton = this;
        }
        else if (s_Singleton != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        myAudioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void PlaySFX (AudioClip currentSFX)
    {
        myAudioSource.PlayOneShot(currentSFX);
    }
}
