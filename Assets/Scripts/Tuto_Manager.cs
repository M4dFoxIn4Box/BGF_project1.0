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

    public void ActivatingTuto(int tutoToActive)
    {
        if (tutoHasBeenDone[tutoToActive] == false)
        {       
            currentScriptableTuto = tutoList[tutoToActive];
            tutoTitleTxt.text = currentScriptableTuto.tutoTitle;
            menuTuto.SetActive(true);
            tutoImg.sprite = currentScriptableTuto.tutoImageBoard[tutoIdx];
            tutoTxt.text = currentScriptableTuto.tutoTextBoard[tutoIdx];
            tutoHasBeenDone[tutoToActive] = true;
        }
    }

    public void MoveToNextSlide ()
    {
        Debug.Log(currentScriptableTuto.numberOfSlides);

        if(tutoIdx == currentScriptableTuto.numberOfSlides - 1)
        {
            tutoIdx = 0;
            menuTuto.SetActive(false);
            Debug.Log("Done");
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
        for (int k = 0; k < isTutoCheck.Count; k++)
        {
            //.GetChild(j).gameObject.GetComponent<Image>().enabled = isTutoCheck[k];
        }
    }

}

