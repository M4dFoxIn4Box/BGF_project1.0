using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeBehaviour { Toggle, LeftOrRight, Up}
public enum SpawnOrientation { Horizontal, Vertical}

//[RequireComponent(typeof(Rigidbody))]
public class InteractionSwipe : MonoBehaviour
{
    public SwipeBehaviour swipeAction;
    public SpawnOrientation spawnOrientation;

    [Tooltip("0 = infinite")]
    public int swipeAttempts = 0;
    public float swipeDistanceThreshold = 3;
    private bool swipeRight = true;
    
    public bool startsInteractable = false;
    private Animator myAnim;
    private bool canSwipe = false;

    private Rigidbody myRb;

    private Vector2 startPosition;
    private Vector2 endPosition;
    

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        myRb = GetComponent<Rigidbody>();
        if (startsInteractable)
        {
            EnableInteractionSwipe();
        }

        if (spawnOrientation == SpawnOrientation.Vertical)
        {
            Quaternion newrot = new Quaternion(0.7f, 0, 0, 0.7f);
            transform.localRotation = newrot;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canSwipe)
        {
            //if (interactionType == InteractionTypes.SwipeTurn && Input.touchCount == 1)
            //{
            //    var touch = Input.touches[0];
            //    switch (touch.phase)
            //    {
            //        case TouchPhase.Began:
            //            // Stockage du point de départ
            //            startPosition = touch.position;
            //            break;
            //        case TouchPhase.Ended:
            //            // Stockage du point de fin
            //            endPosition = touch.position;
            //            if (Vector2.Distance(startPosition, endPosition) > swipeDistanceThreshold)
            //            {
            //                if (endPosition.x > startPosition.x)
            //{
            //    myRb.AddRelativeTorque(-Vector3.up, ForceMode.Impulse);
            //}
            //        else if (endPosition.x < startPosition.x)
            //{
            //    myRb.AddRelativeTorque(Vector3.up, ForceMode.Impulse);
            //}
            //            }
            //            break;
            //    }
            //}

            //switch (swipeAction)
            //{
            //    case SwipeBehaviour.Toggle:
            //        if (Input.GetMouseButtonDown(0))
            //        {
            //            startPosition = Input.mousePosition;
            //        }
            //        else if (Input.GetMouseButtonUp(0))
            //        {
            //            endPosition = Input.mousePosition;
            //            if (Vector2.Distance(startPosition, endPosition) > swipeDistanceThreshold)
            //            {
            //                ToggleSwipeDirection();
            //            }
            //        }
            //        break;
            //    case SwipeBehaviour.LeftOrRight:
            //        if (Input.GetMouseButtonDown(0))
            //        {
            //            startPosition = Input.mousePosition;
            //        }
            //        else if (Input.GetMouseButtonUp(0))
            //        {
            //            endPosition = Input.mousePosition;
            //            if (Vector2.Distance(startPosition, endPosition) > swipeDistanceThreshold)
            //            {
            //                if (endPosition.x > startPosition.x)
            //                {
            //                    myRb.AddRelativeTorque(-Vector3.up, ForceMode.Impulse);
            //                }
            //                else if (endPosition.x < startPosition.x)
            //                {
            //                    myRb.AddRelativeTorque(Vector3.up, ForceMode.Impulse);
            //                }
            //            }
            //        }
            //        if (myRb.angularVelocity.y > 10f)
            //        {
            //            Vector3 rbav = new Vector3(0, 10, 0);
            //            myRb.angularVelocity = rbav;
            //        }
            //        break;
            //    case SwipeBehaviour.Up:
            //        break;
            //    default:
            //        break;
            //}
        }
    }

    public void EnableInteractionSwipe ()
    {
        canSwipe = true;
    }

    public void DisableInteractionSwipe()
    {
        canSwipe = false;
    }
    
    public void ToggleSwipeDirection ()
    {
        if ((swipeRight && endPosition.x > startPosition.x) || (!swipeRight && endPosition.x < startPosition.x))
        {
            TriggerSwipeAnimation();
            swipeRight = !swipeRight;
        }
    }

    public void TriggerSwipeAnimation ()
    {
        myAnim.SetTrigger("Step");
    }
}
