using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingManager : MonoBehaviour
{

    public GameObject axePrefab;
    public Transform axeSpawnPoint;
    public Transform target;
    public Vector2 turnTargetMinMax;
    private bool timerCDOn = false;
    public float cDBetweenActions = 0.3f;
    private float currentActionCD = 0;

    public static VikingManager s_Singleton;

    private void Awake()
    {
        if (s_Singleton != null)
        {
            Destroy(gameObject);
        }
        else
        {
            s_Singleton = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Interface_Manager.Instance.DisplayScore();
        //currentActionCD = cDBetweenActions;
        //TurnTargetCooldown();
    }

    // Update is called once per frame
    void Update()
    {
        //if (timerCDOn)
        //{
        //    currentActionCD -= Time.deltaTime;
        //    if (currentActionCD <= 0)
        //    {
        //        timerCDOn = false;
        //        currentActionCD = cDBetweenActions;
        //        TurnTarget();
        //    }
        //}
    }

    public void TurnTargetCooldown()
    {
        timerCDOn = true;
    }

    public void TurnTarget()
    {
        target.GetComponent<Animator>().SetTrigger("TurnFace");
    }

    public void ShootAxe (Transform toParent)
    {
        axeSpawnPoint.rotation = Quaternion.LookRotation(toParent.position - transform.position, Vector3.up);
        GameObject tmpAxe = Instantiate(axePrefab, axeSpawnPoint.position, Quaternion.identity);
        tmpAxe.GetComponent<Axe>().FlyTo(toParent);
    }
}
