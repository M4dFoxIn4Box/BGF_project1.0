using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{

    public GameObject cameraAR;
    public Camera mainCamera;
    public Transform spanwPointCamera;
    public GameObject vumarkBGF;
    private Camera tg;


    // Use this for initialization

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetActiveARCamera()
    {
        vumarkBGF.SetActive(true);
        mainCamera.enabled = false;
        cameraAR.SetActive(true);
    }

    public void CloseARCamera()
    {
        mainCamera.enabled = true;
        vumarkBGF.SetActive(false);
        cameraAR.SetActive(false);
    }
}
