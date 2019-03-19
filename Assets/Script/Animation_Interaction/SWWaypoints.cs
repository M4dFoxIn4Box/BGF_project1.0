using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWWaypoints : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter (Collider other)
    {
        XWing tmpXw = other.GetComponent<XWing>();
        if (tmpXw != null)
        {
            tmpXw.PickWPIdx();
        }
    }
}
