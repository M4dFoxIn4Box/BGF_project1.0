using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_Moving_System : MonoBehaviour
{

    public Transform[] wayPoints;
    private int destPoint = 0;
    private NavMeshAgent civilAgent;
    private bool hasBeenShot = false;


    void Start()
    {
        civilAgent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        civilAgent.autoBraking = false;

        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (wayPoints.Length == 0)
            return;

        // Set the civilAgent to go to the currently selected destination.
        civilAgent.destination = wayPoints[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % wayPoints.Length;
    }

    private void FixedUpdate()
    {

    }

    void Update()
    {
        // Choose the next destination point when the civilAgent gets
        // close to the current one.
        if (!civilAgent.pathPending && civilAgent.remainingDistance < 0.5f && !hasBeenShot)
            GotoNextPoint();
    }
}

