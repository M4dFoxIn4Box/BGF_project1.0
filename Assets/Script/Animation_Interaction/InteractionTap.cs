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

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        if (startsInteractable)
        {
            EnableInteractionTap();
        }
        
        Quaternion newrot = new Quaternion(0.7f, 0, 0, 0.7f);
        transform.parent.localRotation = newrot;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("Test");
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
}
