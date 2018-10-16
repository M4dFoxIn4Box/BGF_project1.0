using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VumarkZoner : MonoBehaviour {

    public int imageNumber;

    public void Awake()
    {
       
    }
    // Use this for initialization
    void Start ()
    {
        Interface_Manager.Instance.MapActivation(imageNumber);
        Save_Manager.saving.ImageToTrue(imageNumber);
    }

}
