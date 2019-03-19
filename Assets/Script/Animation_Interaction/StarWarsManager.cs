using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarWarsManager : MonoBehaviour
{
    public GameObject xWingPrefab;
    public Transform waypointsParent;
    public Transform xWingSpawnpoints;
    public float initialSpawnTimer = 2.5f;
    public Transform laserParent;
    public GameObject laserPrefab;
    private bool timerCDOn = false;
    private float currentSpawnCD = 0;

    public static StarWarsManager s_Singleton;

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
        currentSpawnCD = initialSpawnTimer;
        SpawnXWingCooldown();
    }

    public void SpawnXWingCooldown()
    {
        timerCDOn = true;
    }

    public void SpawnXWing()
    {
        int tmpIdx = Random.Range(0, xWingSpawnpoints.childCount);
        GameObject tmpXW = Instantiate(xWingPrefab, xWingSpawnpoints.GetChild(tmpIdx).position, xWingSpawnpoints.GetChild(tmpIdx).rotation, xWingSpawnpoints.GetChild(tmpIdx));
        tmpXW.GetComponent<XWing>().RegisterWPParent(waypointsParent);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerCDOn)
        {
            currentSpawnCD -= Time.deltaTime;
            if (currentSpawnCD <= 0)
            {
                timerCDOn = false;
                currentSpawnCD = initialSpawnTimer;
                SpawnXWing();
            }
        }
    }

    public void FireLaser (Transform currentTarget)
    {
        laserParent.rotation = Quaternion.LookRotation(currentTarget.position - transform.position, Vector3.up);
        Destroy(Instantiate(laserPrefab, laserParent.position, laserParent.rotation, laserParent), 0.3f);
        SpawnXWingCooldown();
    }
}
