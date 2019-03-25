using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Opening : MonoBehaviour
{

    public float timerBeforeLoadingGame = 0.1f;
    private float timerValue = 0.2f;
    private bool timerOn = false;

    public static UIManager_Opening s_Singleton { get; private set; }

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
        timerValue = timerBeforeLoadingGame;
        timerOn = true;
    }
    
    IEnumerator LoadGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            timerValue -= Time.deltaTime;
            if (timerValue < 0)
            {
                timerValue = 0;
                timerOn = false;
                StartCoroutine("LoadGameScene");
            }
        }
    }
}
