using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InfinityStones { Pierre01, Pierre02, Pierre03, Pierre04, Pierre05, Pierre06 }

public class DragDropElement : MonoBehaviour
{
    public InteractionTap myITap;
    public static int stonesSetNb = 0;

    public enum DragOrDrop { Drag, Drop}
    public DragOrDrop behaviourType;

    public InfinityStones infinityStoneNb;
    public float dragDistanceToCamera = 0.4f;
    private bool canBeDragged = true;
    private bool isDragged = false;
    public static bool isDragging = false;
    public static DragDropElement stoneDragged = null;
    private Vector3 mPos = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        stonesSetNb = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (canBeDragged && isDragged)
        {
            mPos = Input.mousePosition;
            mPos.z = dragDistanceToCamera;
            mPos = Camera.main.ScreenToWorldPoint(mPos);
            transform.position = mPos;
        }
    }

    private void OnMouseDown()
    {
        if (behaviourType == DragOrDrop.Drag && canBeDragged)
        {
            isDragged = true;
            isDragging = true;
            stoneDragged = this;
            stoneDragged.GetComponent<Collider>().enabled = false;
        }
    }

    private void OnMouseEnter()
    {
        if (behaviourType == DragOrDrop.Drop)
        {
            if (isDragging && behaviourType == DragOrDrop.Drop)
            {
                if (stoneDragged.infinityStoneNb == infinityStoneNb)
                {
                    stoneDragged.SetOnRightSpot(transform);
                }
            }
        }
    }

    private void OnMouseUp()
    {
        if (behaviourType == DragOrDrop.Drag)
        {
            isDragged = false;
            isDragging = false;
            if (stoneDragged != null)
            {
                stoneDragged.GetComponent<Collider>().enabled = true;
                stoneDragged = null;
            }
        }
    }

    public void SetOnRightSpot (Transform rightSpot)
    {
        canBeDragged = false;
        transform.position = rightSpot.position;
        transform.rotation = rightSpot.rotation;
        isDragging = false;
        isDragged = false;
        stoneDragged.GetComponent<Collider>().enabled = true;
        stoneDragged = null;
        stonesSetNb++;
        transform.SetParent(myITap.transform);
        if (stonesSetNb == 6)
        {
            myITap.EnableInteractionTap();
            myITap.GetComponent<Collider>().enabled = true;
        }
    }
}
