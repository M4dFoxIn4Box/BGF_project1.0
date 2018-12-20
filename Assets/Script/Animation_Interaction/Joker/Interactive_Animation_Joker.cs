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
        Audio_Manager.audio.SoundsToPlay(audioJoker[0]);
        Audio_Manager.audio.GetComponent<AudioSource>().outputAudioMixerGroup = mixerJoker[0];
    }

    void AudioBatarangImpact()
    {
        Audio_Manager.audio.SoundsToPlay(audioJoker[1]);
        Audio_Manager.audio.GetComponent<AudioSource>().outputAudioMixerGroup = mixerJoker[1];
    }

    void AudioJokerLaugh()
    {
        Audio_Manager.audio.SoundsToPlay(audioJoker[2]);
        Audio_Manager.audio.GetComponent<AudioSource>().outputAudioMixerGroup = mixerJoker[2];
    }
}
