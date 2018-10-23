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

    IEnumerator WaitForVumarkReactivation()
    {
        yield return new WaitForSeconds(0.1f);
        vumarkBGF.SetActive(true);
    }

    public void SetActiveARCamera()
    {
        mainCamera.enabled = false;
        cameraAR.SetActive(true);
        StartCoroutine(WaitForVumarkReactivation());
    }

    public void CloseARCamera()
    {
        mainCamera.enabled = true;
        vumarkBGF.SetActive(false);
        cameraAR.SetActive(false);
    }
}
