using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkToStaticARElement : MonoBehaviour
{

    public Transform correspondingStaticElement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetStaticElement ()
    {
        return correspondingStaticElement;
    }
}
