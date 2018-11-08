using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vumark_Manager : MonoBehaviour {

    public int mapIdx;

    public void Awake()
    {
       
    }
    // Use this for initialization
    void Start ()
    {
        Interface_Manager.Instance.MapActivation(mapIdx);
        //Save_Manager.saving.ImageToTrue(mapIdx);
    }

}
