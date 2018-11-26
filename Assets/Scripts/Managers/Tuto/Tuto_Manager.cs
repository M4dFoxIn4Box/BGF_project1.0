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
    private int tutoIdx;
    public List<bool> tutoHasBeenDone;
    public List<ScriptableTuto> tutoList;
    private int tutoToDeactive;
    public Transform tutoGallery;
    private int idxTuto;

    public int tutoQuizzIdx;

    [Header ("Tuto Gallery Manager")]

    public int currentIdxTuto = 0;

    public List<Transform> currentTutoActivate = new List<Transform>();

    public Transform currentTuto = null;

    public GameObject arrowRight;
    public GameObject arrowLeft;
    public GameObject activeArrows;



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

    public void ActivatingTuto(int tutoToActive)
    {
        idxTuto = tutoToActive;
        tutoToDeactive = tutoToActive;

        if (tutoHasBeenDone[tutoToActive] == false)
        {
            currentScriptableTuto = tutoList[tutoToActive];
            tutoTitleTxt.text = currentScriptableTuto.tutoTitle;
            menuTuto.SetActive(true);
            tutoImg.sprite = currentScriptableTuto.tutoImageBoard[tutoIdx];
            tutoTxt.text = currentScriptableTuto.tutoTextBoard[tutoIdx];
        }

    }

    public void MoveToNextSlide ()
    {
        if (tutoIdx == currentScriptableTuto.numberOfSlides - 1)
        {
            tutoIdx = 0;
            menuTuto.SetActive(false);
            tutoHasBeenDone[tutoToDeactive] = true;
            Save_Manager.saving.TutoIsDone(tutoHasBeenDone);

            if (tutoGallery.GetChild(idxTuto).gameObject.GetComponent<Button>().interactable == false)
            {
                tutoGallery.GetChild(idxTuto).gameObject.GetComponent<Button>().interactable = true;
            }

            if (currentScriptableTuto == tutoList[tutoQuizzIdx])
            {
                Interface_Manager.Instance.OpenARCamera();
            }
        }

        else 
        {
            tutoIdx++;
            tutoImg.sprite = currentScriptableTuto.tutoImageBoard[tutoIdx];
            tutoTxt.text = currentScriptableTuto.tutoTextBoard[tutoIdx];
        } 

    }

    //SAVE AND LOAD TUTO

    public void TutoState(List<bool> isTutoCheck)
    {
        for (int k = 0; k < tutoHasBeenDone.Count; k++)
        {
            tutoHasBeenDone[k] = isTutoCheck[k];
        }
    }

    public void LoadMenuTuto(List<bool> isTutoDone)
    {
        for (int i = 0; i < tutoList.Count; i++)
        {
            tutoGallery.GetChild(i).gameObject.GetComponent<Button>().interactable = isTutoDone[i];
        }
    }


    //GALLERY SLIDER MANAGER 

    public void ShowTuto(Transform tutoToShow)
    {
        if (currentTuto != null)
        {
            currentTuto.gameObject.SetActive(false);
            currentTutoActivate[currentIdxTuto].gameObject.SetActive(false);
            currentIdxTuto = 0;
            currentTutoActivate[currentIdxTuto].gameObject.SetActive(true);
            arrowLeft.SetActive(false);
            arrowRight.SetActive(true);
        }

        currentIdxTuto = 0;
        currentTutoActivate.Clear();
        currentTuto = tutoToShow;
        tutoToShow.gameObject.SetActive(true);
        activeArrows.gameObject.SetActive(true);

        for (int i = 0; i < currentTuto.childCount; i++)
        {
            currentTutoActivate.Add(currentTuto.transform.GetChild(i));
        }
    }

    public void UnShowTuto()
    {
        currentTuto.gameObject.SetActive(false);
        activeArrows.gameObject.SetActive(false);
        currentTuto = null;
        currentIdxTuto = 0;
    }

    public void NextStorySlide()
    {
        if (currentIdxTuto < currentTutoActivate.Count - 1)
        {
            currentTutoActivate[currentIdxTuto].gameObject.SetActive(false);
            currentIdxTuto++;
            currentTutoActivate[currentIdxTuto].gameObject.SetActive(true);
            arrowLeft.SetActive(true);
        }

        if (currentIdxTuto == currentTutoActivate.Count - 1)
        {
            arrowRight.SetActive(false);
        }
    }

    public void PrecedentStorySlide()
    {
        if (currentIdxTuto > 0)
        {
            currentTuto.GetChild(currentIdxTuto).gameObject.SetActive(false);
            currentIdxTuto--;
            currentTuto.GetChild(currentIdxTuto).gameObject.SetActive(true);
            arrowRight.SetActive(true);
        }

        if (currentIdxTuto == 0)
        {
            arrowLeft.SetActive(false);
        }
    }
}

