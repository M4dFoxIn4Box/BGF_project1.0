using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{


    private float force;
    Vector3 startPos;
    Vector3 endPos;
    Vector3 direction;
    public float arc;
    private bool IsDrag = false;
    private float distance;
    public int vitesse;
    private float timeClick;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (IsDrag)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = Vector3.Lerp(transform.position, rayPoint, vitesse * Time.deltaTime);
            timeClick += Time.deltaTime;

        }

    }

    void OnMouseDown()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        startPos = Input.mousePosition;
        startPos.z = transform.position.z - Camera.main.transform.position.z;
        startPos = Camera.main.ScreenToWorldPoint(startPos);
        Debug.Log("the mouse down pos is " + startPos);
        IsDrag = true;
    }

    void OnMouseUp()
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        endPos = Input.mousePosition;
        endPos.z = transform.position.z - Camera.main.transform.position.z;
        endPos = Camera.main.ScreenToWorldPoint(endPos);
        Debug.Log("the mouse up pos is " + endPos);
        direction = endPos - startPos;
        direction.Normalize();
        force = ((endPos.y - startPos.y) / timeClick);
        Vector3 tmpDir = new Vector3(direction.x, arc, direction.y);
        GetComponent<Rigidbody>().AddForce(tmpDir * force, ForceMode.Impulse);
        IsDrag = false;
        timeClick = 0f;
    }
}

