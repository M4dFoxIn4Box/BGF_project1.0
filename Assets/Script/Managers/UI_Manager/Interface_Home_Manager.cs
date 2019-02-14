using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Interface_Home_Manager : MonoBehaviour
{
    [Header("Menu")]

    private int currentIdxMenu = 1;
    public GameObject[] menuToActivate;
    public Slider loadingSprite;

    #region Sounds
    [Header("Sounds")]

    public AudioClip audioChangeMenu;
    public AudioMixerGroup[] mixerGroupChangeMenu;

    public AudioSource musicMainMenuToDeactivate;
    private bool stopMusicInGallery = false;

    #endregion

    #region Interface Manager

    public void ChangeMenu(int newIdxMenu)
    {
        //Audio_Manager.audio.SoundsToPlay(audioChangeMenu);
        //Audio_Manager.audio.GetComponent<AudioSource>().outputAudioMixerGroup = mixerGroupChangeMenu[0];
        menuToActivate[currentIdxMenu].SetActive(false);
        menuToActivate[newIdxMenu].SetActive(true);
        currentIdxMenu = newIdxMenu;

        if (stopMusicInGallery)
        {
            DeactiveMusicMainMenu();
        }
    }

    public void ShowElement(GameObject elementToActive)
    {
        Debug.Log("Here");
        elementToActive.SetActive(true);
    }

    public void UnShowElement(GameObject elementToDesactive)
    {
        Debug.Log("Here");
        elementToDesactive.SetActive(false);
    }


    public void DeactiveMusicMainMenu()//Musique à désactiver ou activer dans la galerie
    {
        Debug.Log("Here");
        if (stopMusicInGallery)
        {
            musicMainMenuToDeactivate.UnPause();
            stopMusicInGallery = false;
        }
        else if (!stopMusicInGallery)
        {
            musicMainMenuToDeactivate.Pause();
            stopMusicInGallery = true;
        }
    }
    #endregion

    // Use this for initialization
    void Start()
    {

    }

    public void QuitAPK()
    {
        Application.Quit();
    }

    public void MoveToBGFScene()
    {
        StartCoroutine(LoadBGFScene());
    }

    IEnumerator LoadBGFScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene_BGF");
        while (!asyncLoad.isDone)
        {
            loadingSprite.value = SceneManager.LoadSceneAsync("Scene_BGF").progress;
            yield return null;
        }
    }
}

