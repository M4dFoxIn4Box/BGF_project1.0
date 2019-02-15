using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tuto_Manager : MonoBehaviour
{
    public ScriptableTuto currentScriptableTuto;
    public GameObject menuTuto;
    public Text tutoTitleTxt;
    public Image tutoImg;
    public Text tutoTxt;
    private int currentSlideIdx;
    public List<bool> tutoHasBeenDone;
    public List<ScriptableTuto> tutoList;
    private int tutoToDeactive;
    private int idxTuto;
    private bool activateARCamera = true;


    public int tutoQuizzIdx;

    [Header ("Tuto Gallery Manager")]

    public Transform tutoGallery;
    public GameObject buttonPrecedentSlide;



    public static Tuto_Manager tuto {get; private set;}

    private void Awake()
    {
        if (tuto == null)
        {
            DontDestroyOnLoad(gameObject);
            tuto = this;
        }
        else if (tuto != this)
        {
            Destroy(gameObject);
        }
    }

    public void ActivatingTutoGallery(int tutoNumber)//active les tuto depuis la galerie (seul changement le bool)
    {
        Debug.Log("Here");
        idxTuto = tutoNumber;
        tutoToDeactive = tutoNumber;
        currentScriptableTuto = tutoList[tutoNumber];
        tutoTitleTxt.text = currentScriptableTuto.tutoTitle;
        menuTuto.SetActive(true);
        tutoImg.sprite = currentScriptableTuto.tutoImageBoard[currentSlideIdx];
        tutoTxt.text = currentScriptableTuto.tutoTextBoard[currentSlideIdx];
    }

    public void ActivatingTuto(int tutoToActive)//active un tuto et verifie s'il a été fait
    {
        Debug.Log("Here");
        idxTuto = tutoToActive;
        tutoToDeactive = tutoToActive;
        if (tutoHasBeenDone[tutoToActive] == false)
        {
            currentScriptableTuto = tutoList[tutoToActive];
            tutoTitleTxt.text = currentScriptableTuto.tutoTitle;
            menuTuto.SetActive(true);
            tutoImg.sprite = currentScriptableTuto.tutoImageBoard[currentSlideIdx];
            tutoTxt.text = currentScriptableTuto.tutoTextBoard[currentSlideIdx];
        }
    }

    public void MoveToNextSlide ()
    {
        Debug.Log("Here");
        if (currentSlideIdx == currentScriptableTuto.numberOfSlides - 1)
        {
            currentSlideIdx = 0;
            menuTuto.SetActive(false);
            tutoHasBeenDone[tutoToDeactive] = true;
            //Save_Manager.saving.TutoIsDone(tutoHasBeenDone);
            buttonPrecedentSlide.SetActive(false);

            if (tutoGallery.GetChild(idxTuto).gameObject.GetComponent<Button>().interactable == false)
            {
                tutoGallery.GetChild(idxTuto).gameObject.GetComponent<Button>().interactable = true;
            }

            if (currentScriptableTuto == tutoList[tutoQuizzIdx] && activateARCamera)
            {
                Interface_Manager.Instance.OpenARCamera();
                activateARCamera = false;
                //Save_Manager.saving.ARCameraTuto(activateARCamera);
            }

        }

        else 
        {
            buttonPrecedentSlide.SetActive(true);
            currentSlideIdx++;
            tutoImg.sprite = currentScriptableTuto.tutoImageBoard[currentSlideIdx];
            tutoTxt.text = currentScriptableTuto.tutoTextBoard[currentSlideIdx];
        } 

    }

    public void MoveToPrecedentSlide()
    {
        Debug.Log("Here");
        if (currentSlideIdx != 0)
        {
            currentSlideIdx--;
            tutoImg.sprite = currentScriptableTuto.tutoImageBoard[currentSlideIdx];
            tutoTxt.text = currentScriptableTuto.tutoTextBoard[currentSlideIdx];
            if(currentSlideIdx == 0)
            {
                buttonPrecedentSlide.SetActive(false);
            }
        }
    }

    //SAVE AND LOAD TUTO & ARCAMERA IN GALLERY

    public void TutoState(List<bool> isTutoCheck)
    {
        Debug.Log("Here");
        for (int k = 0; k < tutoHasBeenDone.Count; k++)
        {
            tutoHasBeenDone[k] = isTutoCheck[k];
        }
    }

    public void LoadMenuTuto(List<bool> isTutoDone)
    {
        Debug.Log("Here");
        for (int i = 0; i < tutoList.Count; i++)
        {
            tutoGallery.GetChild(i).gameObject.GetComponent<Button>().interactable = isTutoDone[i];
        }
    }

    public void LoadBoolForTuto(bool ARCameraTutoGalley)
    {
        Debug.Log("Here");
        activateARCamera = ARCameraTutoGalley;
    }
}

