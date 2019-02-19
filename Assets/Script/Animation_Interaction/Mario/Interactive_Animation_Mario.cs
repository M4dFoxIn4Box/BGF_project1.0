using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Interactive_Animation_Mario : MonoBehaviour {

    public Animator anim;

    public GameObject champignon;
    public AudioClip[] audioMario;
    public AudioMixerGroup[] mixerMario;

    // Use this for initialization
    void Start ()
    {

        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        anim.SetBool("AnimPlay", true);
    }

    void ActiveChampi()
    {
        champignon.SetActive(true);
    }

    void AudioJump()
    {
        AudioManager.s_Singleton.PlaySFX(audioMario[0]);
        AudioManager.s_Singleton.GetComponent<AudioSource>().outputAudioMixerGroup = mixerMario[0];
    }

    void AudioPowerUP()
    {
        AudioManager.s_Singleton.PlaySFX(audioMario[1]);
        AudioManager.s_Singleton.GetComponent<AudioSource>().outputAudioMixerGroup = mixerMario[1];
    }
}
