using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingManager : MonoBehaviour
{

    public GameObject axePrefab;
    public Transform axeSpawnPoint;
    public Transform targets;
    private int currentActivePhase = -1;
    private int nbOfIdleTargets = 0;
    private List<int> currentTurnedTargets = new List<int>();
    private List<GameObject> waveThrownAxes = new List<GameObject>();

    public List<GamePhases> vikingsGamePhases;

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
        Interface_Manager.Instance.SetupGame(0);
        for (int i = 0; i < vikingsGamePhases.Count; i++)
        {
            vikingsGamePhases[i].isActive = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckTimerAndSwitchPhase();
    }

    public void CheckTimerAndSwitchPhase ()
    {
        int currentTimer = Interface_Manager.Instance.GetTimerValue();

        for (int i = 0; i < vikingsGamePhases.Count; i++)
        {
            if (i + 1 < vikingsGamePhases.Count)
            {
                if (currentTimer < vikingsGamePhases[i].triggerTime && currentTimer > vikingsGamePhases[i+1].triggerTime && !vikingsGamePhases[i].isActive)
                {
                    currentActivePhase = i;
                    vikingsGamePhases[i].isActive = true;
                    if (i == 0)
                    {
                        TurnTargets();
                    }
                }
            }
            else if (i + 1 >= vikingsGamePhases.Count)
            {
                if (currentTimer < vikingsGamePhases[i].triggerTime && currentTimer > 0 && !vikingsGamePhases[i].isActive)
                {
                    currentActivePhase = i;
                    vikingsGamePhases[i].isActive = true;
                }
            }
        }
    }

    public void TurnTargets()
    {
        for (int i = 0; i < vikingsGamePhases[currentActivePhase].targetsNumber; i++)
        {
            int rndTarget = Random.Range(0, targets.childCount);
            while (currentTurnedTargets.Contains(rndTarget))
            {
                rndTarget = Random.Range(0, targets.childCount);
            }
            currentTurnedTargets.Add(rndTarget);
            targets.GetChild(rndTarget).GetComponent<TargetRoot>().TriggerTarget(vikingsGamePhases[currentActivePhase].availableTimeToAct);
        }
    }

    public void TriggerNextTargetsWave ()
    {
        nbOfIdleTargets++;
        if (nbOfIdleTargets == currentTurnedTargets.Count)
        {
            currentTurnedTargets.Clear();
            for (int i = 0; i < waveThrownAxes.Count; i++)
            {
                Destroy(waveThrownAxes[i]);
            }
            TurnTargets();
            nbOfIdleTargets = 0;
        }
    }

    public void ShootAxe (Transform toParent)
    {
        axeSpawnPoint.rotation = Quaternion.LookRotation(toParent.position - transform.position, Vector3.up);
        GameObject tmpAxe = Instantiate(axePrefab, axeSpawnPoint.position, Quaternion.identity);
        tmpAxe.GetComponent<Axe>().FlyTo(toParent);
    }

    public void AddAxeToDestroyList (GameObject shotAxe)
    {
        waveThrownAxes.Add(shotAxe);
    }

    public void StopGame ()
    {
        foreach (Transform target in targets)
        {
            target.GetComponent<TargetRoot>().EndGame();
        }
    }
}
