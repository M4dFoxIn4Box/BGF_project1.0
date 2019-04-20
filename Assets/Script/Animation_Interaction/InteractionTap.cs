using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTap : MonoBehaviour
{
    private bool canTap = false;
    public bool startsInteractable = false;
    private Animator myAnim;
    
    [Tooltip("0 = infinite")]
    public int tapAttempts = 0;
    private int currentTapAttempts = 0;

    public bool triggerOnlyAdditionalAnim = false;
    public Animator additionalAnimatorToTrigger;

    public InteractionTap nextTapToTrigger;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        if (startsInteractable)
        {
            EnableInteractionTap();
        }       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUpAsButton()
    {
        if (canTap)
        {
            if (triggerOnlyAdditionalAnim)
            {
                TriggerAdditionalAnimation();
                canTap = false;
                return;
            }
            TriggerTapAnimation();
        }
    }

    public void TriggerTapAnimation()
    {
        if (tapAttempts == 0)
        {
            if(myAnim != null)
            {
                myAnim.SetTrigger("Step");
            }
            DisableInteractionTap();
        }
        else if (tapAttempts > 0)
        {
            currentTapAttempts++;
            if (myAnim != null)
            {
                myAnim.SetTrigger("Step");
            }
            DisableInteractionTap();
            if (currentTapAttempts >= tapAttempts)
            {
                if (myAnim != null)
                {
                    myAnim.SetTrigger("Reward");
                }
            }
        }
    }

    public void EnableInteractionTap()
    {
        canTap = true;
    }

    public void DisableInteractionTap()
    {
        canTap = false;
    }

    public void TriggerAdditionalAnimation()
    {
        if (additionalAnimatorToTrigger != null)
        {
            additionalAnimatorToTrigger.SetTrigger("Step");
        }
    }

    public void TriggerNextTap ()
    {
        nextTapToTrigger.EnableInteractionTap();
    }
}
