using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tuto_Manager : MonoBehaviour
{
    public ScriptableTuto currentScriptableTuto;
    public GameObject tutoInterface;
    public Image tutoImg;
    public Text tutoTxt;
    private int tutoIdx;

    public void ActivatingTuto (ScriptableTuto tutoToActive)
    {
        currentScriptableTuto = tutoToActive;
        tutoInterface.SetActive(true);
        tutoImg.sprite = currentScriptableTuto.tutoImageBoard[tutoIdx];
        tutoTxt.text = currentScriptableTuto.tutoTextBoard[tutoIdx];
    }

    public void MoveToNextSlide ()
    {
        Debug.Log(currentScriptableTuto.numberOfSlides);

        if(tutoIdx == currentScriptableTuto.numberOfSlides - 1)
        {
            tutoIdx = 0;
            tutoInterface.SetActive(false);
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





//    [Header("Tuto")]

//    public Transform tutoPannel;
//    private List<int> tutoIdx = new List<int>();

//    // Use this for initialization
//    void Start () {
		
//	}
	
//	// Update is called once per frame
//	void Update () {
		
//	}


//    public void DestroyTuto(int idxTuto)//Manager tuto to destroy
//    {
//        if (!tutoIdx.Contains(idxTuto))
//        {
//            tutoIdx.Add(idxTuto);
//            if (tutoPannel.GetChild(idxTuto).gameObject)
//            {
//                tutoPannel.GetChild(idxTuto).gameObject.SetActive(false);
//                Save_Manager.saving.TutoToDestroy(idxTuto);
//            }
//        }
//    }

//    public void LoadingTutoToDestroy(List<bool> isTutoCheck)//Load tuto to destroy
//    {
//        for (int k = 0; k < isTutoCheck.Count; k++)
//        {
//            if (tutoPannel.GetChild(k).gameObject == true)
//            {
//                tutoPannel.GetChild(k).gameObject.SetActive(isTutoCheck[k]);
//            }
//        }
//    }
//}

	//public Transform tutoB;
 //   public Transform tutoS;
 //   public Transform tutoI;
 //   public Transform tutoG;
 //   public GameObject arrowRight;
 //   public GameObject arrowLeft;
 //   public GameObject activeArrows;

 //   private Transform currentTuto = null;
 //   private int index = 0;

 //   private List<Transform> children = new List<Transform>();

 //   public void ShowElement(GameObject elementToActive)
 //   {
 //       elementToActive.SetActive(true);
 //   }

 //   public void UnShowElement(GameObject elementToDesactive)
 //   {
 //       elementToDesactive.SetActive(false);
 //   }

 //   public void ShowTuto(Transform tutoToShow)
 //   {
 //       if (currentTuto != null)
 //       {
 //           currentTuto.gameObject.SetActive(false);
 //           children[index].gameObject.SetActive(false);
 //           index = 0;
 //           children[index].gameObject.SetActive(true);
 //           arrowLeft.SetActive(false);
 //           arrowRight.SetActive(true);
 //       }

 //       index = 0;
 //       children.Clear();
 //       currentTuto = tutoToShow;
 //       tutoToShow.gameObject.SetActive(true);
 //       activeArrows.gameObject.SetActive(true);

 //       for (int i = 0; i < currentTuto.childCount; i++)
 //       {
 //           children.Add(currentTuto.transform.GetChild(i));
 //       }
 //       Debug.Log("children" + children.Count);
 //       Debug.Log(index);
 //   }

 //   public void UnShowTuto()
 //   {
 //       currentTuto.gameObject.SetActive(false);
 //       activeArrows.gameObject.SetActive(false);
 //       currentTuto = null;
 //       index = 0;
 //   }

 //   public void NextTuto()
 //   {
 //       if (index < children.Count - 1)
 //       {
 //           currentTuto.GetChild(index).gameObject.SetActive(false);
 //           index++;
 //           currentTuto.GetChild(index).gameObject.SetActive(true);
 //           arrowLeft.SetActive(true);
 //           Debug.Log(index);
 //       }

 //       if (index == children.Count - 1)
 //       {
 //           arrowRight.SetActive(false);
 //       }
 //   }

 //   public void PreviousTuto()
 //   {

 //       if (index > 0)
 //       {
 //           currentTuto.GetChild(index).gameObject.SetActive(false);

 //           index--;
 //           currentTuto.GetChild(index).gameObject.SetActive(true);
 //           arrowRight.SetActive(true);
 //       }

 //       if (index == 0)
 //       {
 //           arrowLeft.SetActive(false);
 //       }

 //   }


}

