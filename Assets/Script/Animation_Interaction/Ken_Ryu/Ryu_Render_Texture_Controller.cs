using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Ryu_Render_Texture_Controller : MonoBehaviour {

    public VideoPlayer tex;
    public 

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Reset()
    {
        tex.frame = 0;
        tex.Play();
        Debug.Log(tex.frame);
    }

    IEnumerator resetVideo()
    {
        yield return new WaitForSeconds(0.01f);
    }
}
