using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taupiqueur : MonoBehaviour
{
    private bool canBeHit = true;
    private Animator myAnim;

    private void Awake()
    {
        myAnim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerTaupiqueur()
    {
        myAnim.SetTrigger("Step");
        canBeHit = true;
    }

    private void OnMouseDown()
    {
        Debug.Log("Hit");
        if (canBeHit)
        {
            canBeHit = false;
            myAnim.SetTrigger("Hit");
            Interface_Manager.Instance.AddScore();
        }
    }

    public void TaupiqueurHiding ()
    {
        TaupiqueurManager.s_Singleton.SpawnTaupiqueurCooldown();
    }
}
