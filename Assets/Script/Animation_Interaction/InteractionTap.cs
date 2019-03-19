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

    public Animator additionalAnimatorToTrigger;

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
            TriggerTapAnimation();
        }
    }

    public void TriggerTapAnimation()
    {
        if (tapAttempts == 0)
        {
            myAnim.SetTrigger("Step");
            DisableInteractionTap();
        }
        else if (tapAttempts > 0)
        {
            currentTapAttempts++;
            myAnim.SetTrigger("Step");
            DisableInteractionTap();
            if (currentTapAttempts >= tapAttempts)
            {
                myAnim.SetTrigger("Reward");
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
}
