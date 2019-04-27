using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapARSpot : MonoBehaviour
{

    public GameObject spotTofind;
    public GameObject spotFound;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpotToFind ()
    {
        spotTofind.SetActive(true);
        spotFound.SetActive(false);
    }

    public void SetSpotFound()
    {
        spotTofind.SetActive(false);
        spotFound.SetActive(true);
    }

    public bool SpotFound ()
    {
        return spotFound.activeSelf;
    }
}
