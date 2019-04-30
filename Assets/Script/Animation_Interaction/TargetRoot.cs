using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRoot : MonoBehaviour
{

    private Animator myAnim;
    private float myTimer = 0f;
    private bool canPlay = true;
    private bool canShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlay)
        {
            if (myTimer > 0f)
            {
                myTimer -= Time.deltaTime;
                if (myTimer <= 0)
                {
                    myTimer = 0f;
                    TriggerAnim();
                    SetCantShoot();
                }
            }
        }
    }

    private void TriggerAnim ()
    {
        myAnim.SetTrigger("Step");
    }

    public void SetTargetActive()
    {
        foreach (Transform goChild in transform)
        {
            goChild.gameObject.SetActive(true);
        }
        Target[] rootTargets = GetComponentsInChildren<Target>();
        foreach (Target targ in rootTargets)
        {
            targ.SetCanBeHit();
        }
        canShoot = true;
    }

    public void SetTargetInactive ()
    {
        foreach (Transform goChild in transform)
        {
            goChild.gameObject.SetActive(false);
        }
    }

    public void SetCanShoot ()
    {
        canShoot = true;
    }

    public void SetCantShoot()
    {
        canShoot = false;
    }

    public bool GetCanShoot()
    {
        return canShoot;
    }

    public void TriggerTarget (float timeToShoot)
    {
        TriggerAnim();
        myTimer = timeToShoot;
    }

    public void TargetShot ()
    {
        myTimer = Time.deltaTime;
    }

    public void TurnedBackTargetNotifyManager ()
    {
        VikingManager.s_Singleton.TriggerNextTargetsWave();
    }

    public void EndGame ()
    {
        canPlay = false;
        SetTargetInactive();
        canShoot = false;
        myAnim.SetTrigger("EndGame");
    }
}
