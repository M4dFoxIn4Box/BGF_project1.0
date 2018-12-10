using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonPokeball : MonoBehaviour {

    //public Animator pokemonAnim;
    public Animator cameraAnim;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void CameraChangeView()
    {
        cameraAnim.SetBool("ChangeCamera", true);
    }
}
