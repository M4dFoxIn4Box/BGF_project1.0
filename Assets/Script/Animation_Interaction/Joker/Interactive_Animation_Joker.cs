using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Interactive_Animation_Joker : MonoBehaviour {

    public Animator jokerAnimator;

    public AudioClip[] audioJoker;
    public AudioMixerGroup[] mixerJoker;

    // Update is called once per frame
    void Update () {
		
	}

    void OnMouseDown()
    {
        jokerAnimator.SetBool("Batarang", true);
    }

    void AudioBatarangThrow()
    {
        AudioManager.s_Singleton.PlaySFX(audioJoker[0]);
        AudioManager.s_Singleton.GetComponent<AudioSource>().outputAudioMixerGroup = mixerJoker[0];
    }

    void AudioBatarangImpact()
    {
        AudioManager.s_Singleton.PlaySFX(audioJoker[1]);
        AudioManager.s_Singleton.GetComponent<AudioSource>().outputAudioMixerGroup = mixerJoker[1];
    }

    void AudioJokerLaugh()
    {
        AudioManager.s_Singleton.PlaySFX(audioJoker[2]);
        AudioManager.s_Singleton.GetComponent<AudioSource>().outputAudioMixerGroup = mixerJoker[2];
    }
}
