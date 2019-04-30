using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public float flyingSpeed = 5f;
    private bool isSet = false;
    private Transform newParent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isSet)
        {
            transform.position = Vector3.MoveTowards(transform.position, newParent.position, flyingSpeed * Time.deltaTime);
            if (transform.position == newParent.position)
            {
                isSet = true;
                transform.SetParent(newParent);
                GetComponentInParent<TargetRoot>().TargetShot();
                VikingManager.s_Singleton.AddAxeToDestroyList(gameObject);
            }
        }
    }

    public void FlyTo (Transform myParent)
    {
        newParent = myParent;
    }
}
