using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatableButton : MonoBehaviour
{
    private Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerBadgeOwnedAnimation ()
    {
        myAnim.SetTrigger("Step");
    }

    //Rend le bouton du badge interactif (appelé par l'Animator du bouton)
    public void EnableBadgeButton()
    {
        GetComponent<Button>().interactable = true;
    }
}
