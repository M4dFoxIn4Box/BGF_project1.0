using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaupiqueurManager : MonoBehaviour
{
    public Transform spawnParent;
    public float initialSpawnTimer = 2.5f;
    private float currentSpawnCD = 0f;
    private bool timerCDOn = false;

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
        Interface_Manager.Instance.DisplayScore();
        if (SaveManager.Data.PokemonScore > 0)
        {
            Interface_Manager.Instance.AddScore(SaveManager.Data.PokemonScore);
        }
        currentSpawnCD = initialSpawnTimer;
        SpawnTaupiqueurCooldown();
    }

    public void SpawnTaupiqueurCooldown ()
    {
        timerCDOn = true;
    }

    public void SpawnTaupiqueur()
    {
        spawnParent.GetChild(Random.Range(0, spawnParent.childCount)).GetComponentInChildren<Taupiqueur>().TriggerTaupiqueur();
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
                SpawnTaupiqueur();
            }
        }
    }

    private void OnDestroy()
    {
        int pcScore = Interface_Manager.Instance.GetCurrentScore();
        if (pcScore > SaveManager.Data.PokemonScore)
        {
            SaveManager.Data.PokemonScore = pcScore;
            SaveManager.SaveToFile();
        }
    }
        
}
