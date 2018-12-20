using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;

public class Interactive_Animation_Zelda : MonoBehaviour {

    public Animator currentAnimation;
    private bool isOKTap = true;
    public GameObject fakeARObject;
    public AudioClip[] audioZelda;
    public AudioMixerGroup[] mixerZelda;
    private int excaliburCount = 3;
    
	// Use this for initialization
	void Start ()
    {
        //ScriptTracker.Instance.FakeARToDeactivate(fakeARObject);
        currentAnimation = GetComponent<Animator>();
        excaliburCount = 3;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        
    }

    void OnMouseDown()
    {
        if(isOKTap)
        {
            Debug.Log(excaliburCount);
            if (excaliburCount == 1)
            {
                currentAnimation.SetTrigger("Excalibur_Make_Step");
                excaliburCount--;
                Audio_Manager.audio.SoundsToPlay(audioZelda[1]);
                Audio_Manager.audio.GetComponent<AudioSource>().outputAudioMixerGroup = mixerZelda[1];
                StartCoroutine(GetExcalibur());
            }
            else if(excaliburCount >= 1)
            {   excaliburCount--;
                Audio_Manager.audio.SoundsToPlay(audioZelda[0]);
                Audio_Manager.audio.GetComponent<AudioSource>().outputAudioMixerGroup = mixerZelda[0];
                isOKTap = false;
                currentAnimation.SetTrigger("Excalibur_Make_Step");
            }
        }
    }

    public void PlayerCanTap()
    {
        isOKTap = true;
    }

    public IEnumerator GetExcalibur()
    {
        Debug.Log("YOOOOOOOOOOOOOOOOOOO");
        yield return new WaitForSeconds(1.0f);
        Audio_Manager.audio.SoundsToPlay(audioZelda[2]);
        Audio_Manager.audio.GetComponent<AudioSource>().outputAudioMixerGroup = mixerZelda[2];
    }

    //public void ExcaliburIsFinished()
    //{
        
    //    Debug.Log(fakeARObject);
    //}
}
