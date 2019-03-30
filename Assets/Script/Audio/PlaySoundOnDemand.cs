using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnDemand : MonoBehaviour
{
    private AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound (AudioClip soundToPlay)
    {
        myAudioSource.clip = soundToPlay;
        myAudioSource.Play();
    }
}
