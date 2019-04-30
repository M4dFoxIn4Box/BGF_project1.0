using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private bool canBeHit = true;
    public int reward = 50;
    private TargetRoot myTarget;

    // Start is called before the first frame update
    void Start()
    {
        myTarget = GetComponentInParent<TargetRoot>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCanBeHit ()
    {
        if (!canBeHit)
        {
            canBeHit = true;
        }
    }

    private void OnMouseDown()
    {
        if (canBeHit && myTarget.GetCanShoot())
        {
            canBeHit = false;
            myTarget.SetCantShoot();
            VikingManager.s_Singleton.ShootAxe(transform);
            Interface_Manager.Instance.AddScore(reward);
        }
    }
}
