using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryRA : MonoBehaviour
{
    private Transform childToMove;
    public static Transform mainParent;
    public static bool exchanging;
    private bool move;
    private Transform mainParentChild = null;
    private InteractionSwipe mainSwipe;

    private void Awake()
    {
        if (mainParent == null)
        {
            mainParent = transform.parent.GetChild(0);
        }
        childToMove = transform.GetChild(0);
        mainSwipe = mainParent.GetComponent<InteractionSwipe>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            if (mainSwipe.CanSwipe())
            {
                mainSwipe.DisableInteractionSwipe();
            }
            if (mainParentChild == null)
            {
                mainParentChild = mainParent.GetChild(0);
            }
            mainParentChild.position = Vector3.MoveTowards(mainParentChild.position, transform.position, Time.deltaTime);
            childToMove.position = Vector3.MoveTowards(childToMove.position, mainParent.position, Time.deltaTime);
            if (childToMove.position == mainParent.position)
            {
                Setposition();
            }
        }
    }

    private void OnMouseDown()
    {
        if (!exchanging)
        {
            move = true;
            exchanging = true;
        }
    }

    private void Setposition ()
    {
        mainParentChild.SetParent(transform);
        childToMove.SetParent(mainParent);
        childToMove = mainParentChild;
        mainParentChild = null;
        move = false;
        exchanging = false;
        mainSwipe.EnableInteractionSwipe();
    }
}
