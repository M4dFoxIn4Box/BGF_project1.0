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

    }

    private void OnMouseDown()
    {
        if (canBeHit)
        {
            canBeHit = false;
            GetComponentInParent<TargetRoot>().SetTargetInactive();
            VikingManager.s_Singleton.ShootAxe(transform);
            Interface_Manager.Instance.AddScore(reward);
        }
    }
}
