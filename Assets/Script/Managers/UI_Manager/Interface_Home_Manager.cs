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
    //public GameObject deactiveButtons;
    //public GameObject loadingScreen;

    #region Sounds

    //[Header("Sounds")]

    //public AudioClip audioChangeMenu;
    //public AudioMixerGroup[] mixerGroupChangeMenu;

    //public AudioSource musicMainMenuToDeactivate;
    //private bool stopMusicInGallery = false;

    #endregion

    #region Interface Manager

    //public void ChangeMenu(int newIdxMenu)
    //{
    //    //Audio_Manager.audio.SoundsToPlay(audioChangeMenu);
    //    //Audio_Manager.audio.GetComponent<AudioSource>().outputAudioMixerGroup = mixerGroupChangeMenu[0];
    //    homeScreens[currentScreenIdx].SetActive(false);
    //    homeScreens[newIdxMenu].SetActive(true);
    //    currentScreenIdx = newIdxMenu;

    //    if (stopMusicInGallery)
    //    {
    //        DeactiveMusicMainMenu();
    //    }
    //}

    //public void ShowElement(GameObject elementToActive)
    //{
    //    Debug.Log("Here");
    //    elementToActive.SetActive(true);
    //}

    //public void UnShowElement(GameObject elementToDesactive)
    //{
    //    Debug.Log("Here");
    //    elementToDesactive.SetActive(false);
    //}


    //public void DeactiveMusicMainMenu()//Musique à désactiver ou activer dans la galerie
    //{
    //    Debug.Log("Here");
    //    if (stopMusicInGallery)
    //    {
    //        musicMainMenuToDeactivate.UnPause();
    //        stopMusicInGallery = false;
    //    }
    //    else if (!stopMusicInGallery)
    //    {
    //        musicMainMenuToDeactivate.Pause();
    //        stopMusicInGallery = true;
    //    }
    //}
    #endregion

    // Use this for initialization
    void Start()
    {

    }

    public void QuitAPK()
    {
        Application.Quit();
    }

    IEnumerator LoadBGFScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene_BGF");
        while (!asyncLoad.isDone)
        {
            loadingSprite.fillAmount = asyncLoad.progress;
            yield return null;
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

    public void LoadLevel (int level)
    {
        switch (level)
        {
            case 1:
                DisplayScreen(2);
                StartCoroutine(LoadBordeauxScene());
                break;
            case 2:
                DisplayScreen(2);
                StartCoroutine(LoadBGFScene());
                break;
        }
    } 
}

