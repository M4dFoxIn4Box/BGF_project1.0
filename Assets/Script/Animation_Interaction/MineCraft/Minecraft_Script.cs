﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Minecraft_Script : MonoBehaviour {

    public Animator steveController;
    public GameObject fakeARObject;
    private int minecraftCount = 0;
    public GameObject dimondsInTheSky;
    public GameObject dimondsInTheSky2;
    public GameObject stick;

    public AudioClip[] audioMinecraft;
    public AudioMixerGroup[] mixerMinecraft;

    // Use this for initialization
    void Start () {
        steveController = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        if (minecraftCount >= 3)
        {
            steveController.SetBool("Ending", true);
            dimondsInTheSky.SetActive(false);
            dimondsInTheSky2.SetActive(false);
            stick.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (minecraftCount <= 2)
        {
            steveController.SetBool("Hitting", true);
            steveController.SetBool("Idle", false);
        }

    }

    void AddCount ()
    {
        minecraftCount += 1;
        steveController.SetBool("Idle", true);
        steveController.SetBool("Hitting", false);
    }

    void AudioForge()
    {
        AudioManager.s_Singleton.PlaySFX(audioMinecraft[0]);
        AudioManager.s_Singleton.GetComponent<AudioSource>().outputAudioMixerGroup = mixerMinecraft[0];
    }

    void AudioPickUpObject()
    {
        AudioManager.s_Singleton.PlaySFX(audioMinecraft[1]);
        AudioManager.s_Singleton.GetComponent<AudioSource>().outputAudioMixerGroup = mixerMinecraft[1];
    }

    void AudioOh()
    {
        AudioManager.s_Singleton.PlaySFX(audioMinecraft[2]);
        AudioManager.s_Singleton.GetComponent<AudioSource>().outputAudioMixerGroup = mixerMinecraft[2];
    }
}
