using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAManager : MonoBehaviour
{
    private int targetId;
    private int targetIdUnlockingTeaserGame;
    private int targetIdUnlockingMainGame;
    private GameObject currentDisplayedElement;

    public static RAManager s_Singleton { get; private set; }

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
        targetIdUnlockingTeaserGame = Interface_Manager.Instance.GetTargetIdUnlockingTeaserGame();
        targetIdUnlockingMainGame = Interface_Manager.Instance.GetTargetIdUnlockingMainGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TargetScanned(int targetId)
    {
        //Si je scanne la cible qui débloque le Main Event...
        if (targetId == targetIdUnlockingMainGame)
        {
            //...et si c'est la première fois, je lock le Teaser Event et je débloque le Main Event
            if (!SaveManager.Data.eventMainStarted)
            {
                SaveManager.UnlockEventMain();
            }
            //Et je peux commencer à scanner dans le Main Event
            Interface_Manager.Instance.StartScanning(targetId);
        }
        //Sinon, si je scanne n'importe quelle autre cible...
        else if (targetId != targetIdUnlockingMainGame)
        {
            //...si c'est une cible du Main Event...
            if (targetId < targetIdUnlockingTeaserGame)
            {
                //...si je n'ai pas encore scanné la cible qui débloque le Main Event, j'obtiens un message qui m'indique quelle cible scanner à l'accueil
                if (!SaveManager.Data.eventMainStarted)
                {
                    Interface_Manager.Instance.DisplayScanErrorMessage(0);
                }
                //...si j'ai scanné la cible qui débloque le Main Event, je peux la scanner
                else if (SaveManager.Data.eventMainStarted)
                {
                    Interface_Manager.Instance.StartScanning(targetId);
                }
            }
            //...si c'est une cible du Teaser Event...
            else if (targetId >= targetIdUnlockingTeaserGame)
            {
                //...si le Teaser Event est locké, j'obtiens un message qui m'indique que seules les cibles du Main Event sont désormais disponibles
                if (SaveManager.Data.eventTeaserLocked)
                {
                    Interface_Manager.Instance.DisplayScanErrorMessage(2);
                }
                //...si le Teaser Event n'est pas bloqué...
                else if (!SaveManager.Data.eventTeaserLocked)
                {
                    //...si je scanne la cible qui débloque le Teaser Event...
                    if (targetId == targetIdUnlockingTeaserGame)
                    {
                        //...si le Teaser Event n'a pas encore été débloqué, je le débloque
                        if (!SaveManager.Data.eventTeaserStarted)
                        {
                            SaveManager.UnlockEventTeaser();
                        }
                        //Et je peux commencer à scanner dans le Teaser Event
                        Interface_Manager.Instance.StartScanning(targetId);
                    }
                    //...si je scanne une des cibles du Teaser Event qui ne le débloque pas...
                    else if (targetId > targetIdUnlockingTeaserGame)
                    {
                        //...si le Teaser Event n'a pas encore été débloqué, j'obtiens un message qui m'indique quelle cible scanner et où la trouver
                        if (!SaveManager.Data.eventTeaserStarted)
                        {
                            Interface_Manager.Instance.DisplayScanErrorMessage(1);
                        }
                        //...si le Teaser Event a été débloqué, je peux scanner la cible
                        else if (SaveManager.Data.eventTeaserStarted)
                        {
                            Interface_Manager.Instance.StartScanning(targetId);
                        }
                    }
                }
            }
        }
    }

    public void ActiveAnimation(int targetId) // Fait apparaître les récompenses liées au VuMark scanné
    {
        if (!SaveManager.Data.artefactsUnlocked[targetId])
        {
            int badgesCount = SaveManager.Data.badgesUnlocked.Count;
            SaveManager.Data.badgesUnlocked[targetId] = true;
            SaveManager.Data.artefactsUnlocked[targetId] = true;
            if (targetId == SaveManager.Data.artefactsUnlocked.Count- 3)
            {
                SaveManager.Data.badgesUnlocked[badgesCount - 2] = true;
            }

            if (!SaveManager.Data.badgesUnlocked[badgesCount - 1])
            {
                int afUnlocked = 0;
                for (int i = 0; i < SaveManager.Data.artefactsUnlocked.Count; i++)
                {
                    if (SaveManager.Data.artefactsUnlocked[i])
                    {
                        afUnlocked++;
                    }
                }
                if (afUnlocked == SaveManager.Data.artefactsUnlocked.Count)
                {
                    SaveManager.Data.badgesUnlocked[badgesCount - 1] = true;
                }
            }
            SaveManager.SaveToFile();
        }

        Transform scanTargetParent = transform.GetChild(targetId);
        LinkToStaticARElement lts = scanTargetParent.GetComponent<LinkToStaticARElement>();
        if (lts != null)
        {
            currentDisplayedElement = Instantiate(Interface_Manager.Instance.elementsToSpawn[targetId], lts.GetStaticElement().position, lts.GetStaticElement().rotation, lts.GetStaticElement());
            Interface_Manager.Instance.SpotFound(targetId);
            return;
        }
        currentDisplayedElement = Instantiate(Interface_Manager.Instance.elementsToSpawn[targetId], scanTargetParent.position, scanTargetParent.rotation, scanTargetParent);
        Interface_Manager.Instance.SpotFound(targetId);
    }

    public void DestroyAnimation() // Fait apparaître les récompenses liées au VuMark scanné
    {
        if (currentDisplayedElement != null)
        {
            Destroy(currentDisplayedElement);
        }
        Interface_Manager.Instance.LostTracker();
    }
}
