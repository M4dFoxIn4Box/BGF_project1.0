using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Interactive_Animation_Pokemon : MonoBehaviour {

    public Animator pokemonAnim;
    //public Animator cameraAnim;
    public AudioClip[] audioPokemon;
    public AudioMixerGroup[] mixerPokemon;

    void OnMouseDown ()
    {
        pokemonAnim.SetBool("Lunchpokeball", true);
    }

    void AudioCaught ()
    {
        Audio_Manager.audio.SoundsToPlay(audioPokemon[0]);
        Audio_Manager.audio.GetComponent<AudioSource>().outputAudioMixerGroup = mixerPokemon[0];
    }

    //void CameraChangeView()
    //{
    //    cameraAnim.SetBool("ChangeCamera", true);
    //}

    //void Reverse()
    //{
    //    cameraAnim.SetBool("ReverseBool", true);
    //}
}
