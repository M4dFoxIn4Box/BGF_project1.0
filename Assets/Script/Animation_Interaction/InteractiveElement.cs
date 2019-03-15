using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveElement : MonoBehaviour
{

    private Animator myAnim;
    private bool canInteract = false;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        EnableInteraction();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            myAnim.SetTrigger("Interaction");
            DisableInteraction();
        }
    }

    public void EnableInteraction ()
    {
        canInteract = true;
    }

    public void DisableInteraction()
    {
        canInteract = false;
    }
}
