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
}
