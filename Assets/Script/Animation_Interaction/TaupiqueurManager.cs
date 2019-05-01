using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaupiqueurManager : MonoBehaviour
{
    public Transform spawnParent;
    public float initialSpawnTimer = 2.5f;
    private int currentActivePhase = -1;
    private int nbOfTaupiqueursDown = 0;
    private List<int> currentTaupiqueursOut = new List<int>();

    public List<GamePhases> pokemonGamePhases;

    public static TaupiqueurManager s_Singleton;

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
        Interface_Manager.Instance.SetupGame(1);
        for (int i = 0; i < pokemonGamePhases.Count; i++)
        {
            pokemonGamePhases[i].isActive = false;
        }
        //currentSpawnCD = initialSpawnTimer;
        //CheckHiddenTaupiqueursForNextWave();
    }

    public void CheckHiddenTaupiqueursForNextWave ()
    {
        nbOfTaupiqueursDown++;
        if (nbOfTaupiqueursDown == currentTaupiqueursOut.Count)
        {
            currentTaupiqueursOut.Clear();
            SpawnTaupiqueur();
            nbOfTaupiqueursDown = 0;
        }
    }

    public void SpawnTaupiqueur()
    {
        for (int i = 0; i < pokemonGamePhases[currentActivePhase].targetsNumber; i++)
        {
            int rndTarget = Random.Range(0, spawnParent.childCount);
            while (currentTaupiqueursOut.Contains(rndTarget))
            {
                rndTarget = Random.Range(0, spawnParent.childCount);
            }
            currentTaupiqueursOut.Add(rndTarget);
            spawnParent.GetChild(rndTarget).GetComponentInChildren<Taupiqueur>().TriggerTaupiqueur();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        CheckTimerAndSwitchPhase();
    }

    public void CheckTimerAndSwitchPhase()
    {
        int currentTimer = Interface_Manager.Instance.GetTimerValue();

        for (int i = 0; i < pokemonGamePhases.Count; i++)
        {
            if (i + 1 < pokemonGamePhases.Count)
            {
                if (currentTimer < pokemonGamePhases[i].triggerTime && currentTimer > pokemonGamePhases[i + 1].triggerTime && !pokemonGamePhases[i].isActive)
                {
                    currentActivePhase = i;
                    pokemonGamePhases[i].isActive = true;
                    if (i == 0)
                    {
                        SpawnTaupiqueur();
                    }
                }
            }
            else if (i + 1 >= pokemonGamePhases.Count)
            {
                if (currentTimer < pokemonGamePhases[i].triggerTime && currentTimer > 0 && !pokemonGamePhases[i].isActive)
                {
                    currentActivePhase = i;
                    pokemonGamePhases[i].isActive = true;
                }
            }
        }
    }

    public void StopGame ()
    {
        
    }

    private void OnDestroy()
    {
        Interface_Manager.Instance.EndGame();
    } 
}
