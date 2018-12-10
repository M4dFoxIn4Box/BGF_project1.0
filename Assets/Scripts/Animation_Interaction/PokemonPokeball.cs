using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonPokeball : MonoBehaviour {

    public Animator pokemonAnim;
    public Animator cameraAnim;
    public GameObject fakeARObject;


    // Use this for initialization
    void Start () {

        ScriptTracker.Instance.FakeARToDeactivate(fakeARObject);
    }
	
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
