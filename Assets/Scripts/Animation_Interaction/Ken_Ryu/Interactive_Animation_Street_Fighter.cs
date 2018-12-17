using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Interactive_Animation_Street_Fighter : MonoBehaviour {

    public Animator anim;
    public Animator ryuAnim;
    public GameObject fakeARObject;

    public AudioClip[] audioStreetFighter;
    public AudioMixerGroup[] mixerStreetFighter;

    // Use this for initialization
    void Start ()
    {
        ScriptTracker.Instance.FakeARToDeactivate(fakeARObject);
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        anim.SetBool("Hadoken", true);
        Debug.Log("chage state0");
    }

    void DeathActive ()
    {
        ryuAnim.SetBool("Death_Hadoken", true);
    }
}
