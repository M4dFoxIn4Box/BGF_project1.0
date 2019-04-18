using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XWing : MonoBehaviour
{

    public float speed = 5.0f;
    public float rotateSpeed = 50.0f;
    private Transform waypointsParent;
    private int currentWaypointIdx = 0;
    private int previousWPIdx = -1;

    private bool canBeHit = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(waypointsParent.GetChild(currentWaypointIdx).position - transform.position, Vector3.up), rotateSpeed * Time.deltaTime);
        transform.Translate(transform.forward * Time.deltaTime * speed);
    }

    public void FollowNextWaypoint ()
    {
        while (!CheckWPAccessibility())
        {
            PickWPIdx();
        }
    }

    public void RegisterWPParent (Transform wpp)
    {
        waypointsParent = wpp;
        PickWPIdx();
    }

    public void PickWPIdx ()
    {
        currentWaypointIdx = Random.Range(0, waypointsParent.childCount);
        while (currentWaypointIdx == previousWPIdx)
        {
            currentWaypointIdx = Random.Range(0, waypointsParent.childCount);
        }
        Debug.Log("Je vais vers : " + waypointsParent.GetChild(currentWaypointIdx).name);
        previousWPIdx = currentWaypointIdx;
    }

    public bool CheckWPAccessibility ()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, waypointsParent.GetChild(currentWaypointIdx).position - transform.position, out hit, 15f))
        {
            if (hit.collider.transform == waypointsParent.GetChild(currentWaypointIdx))
            {
                return true;
            }
        }
        return false;
    }

    //private void OnMouseDown()
    //{
    //    Debug.Log("Hit");
    //    if (canBeHit)
    //    {
    //        canBeHit = false;
    //        StarWarsManager.s_Singleton.FireLaser(transform);
    //        Interface_Manager.Instance.AddScore();
    //        Destroy(gameObject, 0.15f);
    //    }
    //}
}
