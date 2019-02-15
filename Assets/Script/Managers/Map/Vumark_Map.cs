using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vumark_Map : MonoBehaviour {

    public int mapIdx;

    void Start ()
    {
        Debug.Log("Here");
        Interface_Manager.Instance.MapActivation(mapIdx);
        //Save_Manager.saving.ImageToTrue(mapIdx);
    }
}
