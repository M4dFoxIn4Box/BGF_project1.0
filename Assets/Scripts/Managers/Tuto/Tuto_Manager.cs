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

    public int tutoQuizzIdx;

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
        Debug.Log(currentScriptableTuto.numberOfSlides);

        if(tutoIdx == currentScriptableTuto.numberOfSlides - 1)
        {
            tutoIdx = 0;
            menuTuto.SetActive(false);
            tutoHasBeenDone[tutoToDeactive] = true;
            Save_Manager.saving.TutoIsDone(tutoHasBeenDone);
            if (currentScriptableTuto == tutoList[tutoQuizzIdx])
            {
                ScriptTracker.Instance.QuizzDisplaying();
            }
        }
        else 
        {
            tutoIdx++;
            tutoImg.sprite = currentScriptableTuto.tutoImageBoard[tutoIdx];
            tutoTxt.text = currentScriptableTuto.tutoTextBoard[tutoIdx];
            Debug.Log(tutoIdx);
        } 

    }

    public void TutoState(List<bool> isTutoCheck)
    {
        for (int k = 0; k < tutoHasBeenDone.Count; k++)
        {
            tutoHasBeenDone[k] = isTutoCheck[k];
        }
    }

}

