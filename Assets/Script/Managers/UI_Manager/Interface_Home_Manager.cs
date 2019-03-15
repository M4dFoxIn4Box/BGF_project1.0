using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class Interface_Home_Manager : MonoBehaviour
{
    [Header("Menu")]

    private int currentScreenIdx = 0;
    private int previousScreenIdx = -1;
    public GameObject[] homeScreens;

    [Header("Loading")]
    public Image loadingSprite;
    
    public void LoadLevel(int level)
    {
        switch (level)
        {
            case 1:
                DisplayScreen(1);
                StartCoroutine(LoadBordeauxScene());
                break;
            case 2:
                DisplayScreen(1);
                StartCoroutine(LoadBGFScene());
                break;
        }
    }

    IEnumerator LoadBordeauxScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene_Bordeaux");
        while (!asyncLoad.isDone)
        {
            loadingSprite.fillAmount = asyncLoad.progress;
            yield return null;
        }
    }

    IEnumerator LoadBGFScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene_BGF_Redo");
        while (!asyncLoad.isDone)
        {
            loadingSprite.fillAmount = asyncLoad.progress;
            yield return null;
        } 
    }

    public void DisplayScreen (int newScreenIdx)
    {
        previousScreenIdx = currentScreenIdx;
        homeScreens[currentScreenIdx].SetActive(false);
        currentScreenIdx = newScreenIdx;
        homeScreens[currentScreenIdx].SetActive(true);
    }

    public void BackToPreviousScreen ()
    {
        homeScreens[currentScreenIdx].SetActive(false);
        currentScreenIdx = previousScreenIdx;
        previousScreenIdx = -1;
        homeScreens[currentScreenIdx].SetActive(true);
    }

    public void QuitAPK()
    {
        Application.Quit();
    }

}

