using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private bool canBeHit = true;
    public int reward = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (canBeHit)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        RaycastHit hit;
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            canBeHit = false;
        //            Debug.Log(reward);
        //            VikingManager.s_Singleton.ShootAxe(hit.point, hit.transform);
        //            Interface_Manager.Instance.AddScore(reward);
        //        }
        //    }
        //}
    }

    private void OnMouseDown()
    {
        if (canBeHit)
        {
            canBeHit = false;
            VikingManager.s_Singleton.ShootAxe(transform);
            Interface_Manager.Instance.AddScore(reward);
        }
    }
}
