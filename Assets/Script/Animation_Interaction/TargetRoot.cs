using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRoot : MonoBehaviour
{

    private Animator myAnim;
    private float myTimer = 0f;
    private bool canPlay = true;

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
                    SetTargetInactive();
                    VikingManager.s_Singleton.TriggerNextTargetsWave();
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
    }

    public void SetTargetInactive ()
    {
        foreach (Transform goChild in transform)
        {
            goChild.gameObject.SetActive(false);
        }
    }

    public void TriggerTarget (float timeToShoot)
    {
        TriggerAnim();
        SetTargetActive();
        myTimer = timeToShoot;
    }

    public void TargetShot ()
    {
        myTimer = Time.deltaTime;
    }

    public void EndGame ()
    {
        canPlay = false;
        SetTargetInactive();
        myAnim.SetTrigger("EndGame");
    }
}
