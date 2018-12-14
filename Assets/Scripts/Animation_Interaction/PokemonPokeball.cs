using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PokemonPokeball : MonoBehaviour {

    public Animator pokemonAnim;
    public Animator cameraAnim;

    public AudioClip[] audioPokemon;
    public AudioMixerGroup[] mixerPokemon;
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnMouseDown ()
    {
        pokemonAnim.SetBool("Lunchpokeball", true);
    }

    void CameraChangeView()
    {
        cameraAnim.SetBool("ChangeCamera", true);
    }

    void Reverse()
    {
        cameraAnim.SetBool("ReverseBool", true);
    }
}
